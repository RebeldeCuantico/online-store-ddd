using Catalog.Application;
using Catalog.Application.DTOs;
using System.Diagnostics;
using Wolverine;

namespace Catalog.Api
{
    public static class CategoryController
    {
        public static WebApplication AddCategoryController(this WebApplication app)
        {
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
            .WithName("GetCategoryById")
            .WithOpenApi(); ;

            app.MapGet("/category", async (IMessageBus bus) =>
            {
                var result = await bus.InvokeAsync<List<CategoryDto>>(new GetCategoriesQuery());
                return Results.Ok(result);
            })
            .WithName("GetAllCategories")
            .WithOpenApi();

            app.MapPost("/category", async (IMessageBus bus, AddCategoryCommand addCategory, ActivitySource activitySource) =>
            {
                using var activity = activitySource.StartActivity("AddCategory");
                activity.SetTag("name", addCategory.name);

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
            .WithOpenApi();

            return app;
        }
    }
}
