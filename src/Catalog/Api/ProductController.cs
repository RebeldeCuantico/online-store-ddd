using Catalog.Application;
using Catalog.Application.DTOs;
using Wolverine;

namespace Catalog.Api
{
    public static class ProductController
    {
        public static WebApplication AddProductController(this WebApplication app)
        {
            app.MapGet("/product/{id}", async (IMessageBus bus, Guid id) =>
            {                
                var result = await bus.InvokeAsync<ProductDto>(new GetProductByIdQuery(id));
                if (result is null) return Results.NotFound();
                return Results.Ok(result);
            })
           .WithName("GetProductById")
           .WithOpenApi(); ;

            app.MapPost("/product", async (IMessageBus bus, AddProductCommand addProduct) =>
            {
                var result = await bus.InvokeAsync<Guid>(addProduct);
                return Results.Created($"/product/{result}", result);
            })
            .WithName("AddProduct")
            .WithOpenApi();

            return app;
        }
    }
}
