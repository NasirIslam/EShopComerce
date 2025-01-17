﻿namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryRequest();
    public record GetProductByCategoryResponse(IEnumerable<Product> products);
    public class GetProductByCategoryEndPoint:ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}",async(ISender sender,string category)=>
            {
                var result = await sender.Send(new GetProductByCategoryQuery(category));
                var response=result.Adapt<GetProductByCategoryResponse>();
                return Results.Ok(response);
            }).WithName("GetPrdouctByCategory")
            .WithDescription("GetPrdocutByCategory Description")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
