using Catalog.Api;
using Catalog.Domain;
using Catalog.Domain.DomainEvents;
using Catalog.Infrastructure.Context;
using Catalog.Infrastructure.Repository;
using Catalog.Infrastructure.Settings;
using Catalog.Workers;
using Common.Domain;
using Common.Infrastructure;
using Consul;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Oakton;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Steeltoe.Extensions.Configuration.ConfigServer;
using System.Diagnostics;
using Wolverine;
using Prometheus;
using Catalog.Infrastructure.Diagnostics;
using Serilog;
using Common.Infrastructure.Logging;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine((options) =>
{
    options.PublishMessage<CategoryModified>().ToLocalQueue("DomainEvents");
    options.LocalQueue("DomainEvents").Sequential();
});

builder.AddConfigServer();

builder.Services.Configure<PostgreSQLSettings>(builder.Configuration.GetSection(nameof(PostgreSQLSettings)));
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection(nameof(RedisSettings)));
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection(nameof(KafkaSettings)));



builder.Services.AddTransient<ActivitySource>((activity) => new ActivitySource("Catalog", "1.0.0"));

builder.Services.AddOpenTelemetry().WithTracing(configure =>
{
    var resource = ResourceBuilder.CreateDefault().AddService("Catalog", "1.0.0");
    var activity = builder.Services.BuildServiceProvider().GetRequiredService<ActivitySource>();
    
    configure.AddOtlpExporter(o =>
    {
        o.Protocol = OtlpExportProtocol.Grpc;
        o.Endpoint = new Uri("http://localhost:4317"); //TODO: meter por config
    }).AddSource(activity.Name)
    .AddHttpClientInstrumentation()
    .AddAspNetCoreInstrumentation()
    .AddNpgsql()
    .AddConsoleExporter()
    .SetResourceBuilder(resource);
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CatalogContext>(options =>
{
    var postgreSqlSettings = builder.Configuration.GetSection(nameof(PostgreSQLSettings))?.Get<PostgreSQLSettings>();
    options.UseNpgsql(postgreSqlSettings.ConnectionString);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    var redisSettings = builder.Configuration.GetSection(nameof(RedisSettings))?.Get<RedisSettings>();
    options.Configuration = $"{redisSettings.HostName},password={redisSettings.Password},ssl=False,abortConnect=False";
    options.InstanceName = redisSettings.InstanceName;
});

builder.Services.AddScoped<IDbContext, CatalogContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IConsumer, KafkaConsumer>();
builder.Services.AddTransient<IProducer, KafkaProducer>();
builder.Services.AddTransient<IServiceBus, KafkaServiceBus>();
builder.Services.AddHostedService<CatalogWorker>();


var consulClient = new ConsulClient();
var registration = new AgentServiceRegistration()
{
    ID = "ms-catalog",
    Name = "catalog",
    Address = "localhost",
    Port = 5000
};

await consulClient.Agent.ServiceRegister(registration);

builder.Logging.ClearProviders();
var logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Filter.ByExcluding(e => e.Properties.ContainsKey("RequestPath") && e.Properties["RequestPath"].ToString().Contains("/metrics"))
    .WriteTo.Console(new ElasticSearchJsonFormatterWithSpan())
    .WriteTo.File(new ElasticSearchJsonFormatterWithSpan(), "../../docker/logs/catalog/catalog-.log", LevelAlias.Minimum, rollingInterval: RollingInterval.Hour)
    .CreateLogger();

builder.Logging.AddSerilog(logger);
builder.Services.AddSingleton<CategoryControllerDiagnostics>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpMetrics();
app.UseMetricServer();

app.AddCategoryController();
app.AddProductController();

return await app.RunOaktonCommands(args);

//app.Run();