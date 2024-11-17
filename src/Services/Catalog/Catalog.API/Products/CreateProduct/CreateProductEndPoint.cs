﻿namespace Catalog.API.Products.CreateProduct
{
    public record CreatePrdouctdRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);
    public record CreateProductResponse(Guid Id);

    public class CreateProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async (CreatePrdouctdRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateProductResult>();
                return Results.Created($"/products/{response.Id}", response);
            })
                .WithName("CreateProduct")
                .Produces<CreateProductResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Product")
                .WithDescription("Create Product");
        }
    }
}
  