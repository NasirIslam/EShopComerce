using BuildingBlocks.CQRS;
namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQuery():IQuery<GetPrdouctResult>;
    public record GetPrdouctResult(IEnumerable<Product> Products);
    internal class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetPrdouctResult>
    {
        public async Task<GetPrdouctResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
           
            var products= await session.Query<Product>().ToListAsync();
            return new GetPrdouctResult(products);
            
        }
    }
}
  