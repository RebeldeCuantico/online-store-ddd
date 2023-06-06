using Catalog.Application;
using Catalog.Domain;
using Catalog.Infrastructure.Context;
using Catalog.Infrastructure.Repository;
using Catalog.Infrastructure.Settings;
using Common.Domain;
using Common.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Oakton;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine();

builder.Services.Configure<PostgreSQLSettings>(builder.Configuration.GetSection(nameof(PostgreSQLSettings)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CatalogContext>(options =>
{
    var postgreSqlSettings = builder.Configuration.GetSection(nameof(PostgreSQLSettings))?.Get<PostgreSQLSettings>();
    options.UseNpgsql(postgreSqlSettings.ConnectionString);
});

builder.Services.AddScoped<IDbContext, CatalogContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/category/{id}", async (IMessageBus bus, Guid id) => 
{
    var result = await bus.InvokeAsync<Category>(new GetCategoryByIdQuery(id));
    return Results.Ok(result);
})
.WithName("GetCategory")
.WithOpenApi(); ;

app.MapGet("/category", async (IMessageBus bus) =>
{
    var result = await bus.InvokeAsync<List<Category>>(new GetCategoriesQuery());
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

return await app.RunOaktonCommands(args);

//app.Run();