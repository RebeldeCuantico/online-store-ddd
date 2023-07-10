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
using Oakton;
using Steeltoe.Extensions.Configuration.ConfigServer;
using Wolverine;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddCategoryController();
app.AddProductController();

return await app.RunOaktonCommands(args);

//app.Run();