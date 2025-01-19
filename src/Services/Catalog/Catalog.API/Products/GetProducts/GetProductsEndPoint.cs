namespace Catalog.API.Products.GetProducts

{
    public record GetProductRequest(int? PageNumber, int? PageSize );
    public record GetProductResponse(IEnumerable<Product> Products);
    public class GetProductsEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters] GetProductRequest request, ISender sender) =>
            {
                var query=request.Adapt<GetProductsQuery>();
                var result = await sender.Send( query);
                var response = result.Adapt<GetProductResponse>();
                return Results.Ok(response);
            }).WithName("GetPrdoucts")
            .Produces<GetProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products")
            .WithDescription("Get Description");

        }

    }
}
