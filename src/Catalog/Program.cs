using Catalog.Application;
using Catalog.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Oakton;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/category", async (IMessageBus bus) =>
{    
   var result = await bus.InvokeAsync<List<Category>>(new GetCategoriesQuery());
    return Results.Ok(result);
})
.WithName("GetAllCategories")
.WithOpenApi();

return await app.RunOaktonCommands(args);

//app.Run();