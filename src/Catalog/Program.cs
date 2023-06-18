using Catalog.Application;
using Catalog.Application.DTOs;
using Catalog.Domain;
using Catalog.Domain.DomainEvents;
using Catalog.Infrastructure.Context;
using Catalog.Infrastructure.Repository;
using Catalog.Infrastructure.Settings;
using Catalog.Workers;
using Common.Domain;
using Common.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Oakton;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine((options) => 
{
    options.PublishMessage<CategoryModified>().ToLocalQueue("DomainEvents");
    options.LocalQueue("DomainEvents").Sequential();
});

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
builder.Services.AddTransient<IConsumer, KafkaConsumer>();
builder.Services.AddTransient<IProducer, KafkaProducer>();
builder.Services.AddTransient<IServiceBus, KafkaServiceBus>();

builder.Services.AddHostedService<CatalogWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/category/{id}", async (IMessageBus bus, Guid id) =>
{
    //TODO: validar datos de entrada
    var result = await bus.InvokeAsync<CategoryDto>(new GetCategoryByIdQuery(id));
    if (result is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(result);
})
.WithName("GetCategory")
.WithOpenApi(); ;

app.MapGet("/category", async (IMessageBus bus) =>
{
    var result = await bus.InvokeAsync<List<CategoryDto>>(new GetCategoriesQuery());
    return Results.Ok(result);
})
.WithName("GetAllCategories")
.WithOpenApi();

app.MapPost("/category", async (IMessageBus bus, AddCategoryCommand addCategory) =>
{
    var result = await bus.InvokeAsync<Guid>(addCategory);
    return Results.Created($"/category/{result}", result);
})
.WithName("AddCategory")
.WithOpenApi();

app.MapDelete("/category/{id}", async (IMessageBus bus, Guid id) =>
{
    var result = await bus.InvokeAsync<Guid>(new DeleteCategoryCommand(id));
    if (result == Guid.Empty)
    {
        return Results.NotFound();
    }

    return Results.Ok(result);
})
.WithName("DeleteCategory")
.WithOpenApi();

app.MapPut("/category", async (IMessageBus bus, UpdateCategoryCommand updateCategoryCommand) =>
{
    var result = await bus.InvokeAsync<CategoryDto>(updateCategoryCommand);
    if (result is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(result);
})
.WithName("UpdateCategory")
.WithOpenApi(); ;

return await app.RunOaktonCommands(args);

//app.Run();