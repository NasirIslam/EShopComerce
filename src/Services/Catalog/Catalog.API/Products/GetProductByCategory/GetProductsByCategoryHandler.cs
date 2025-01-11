using BuildingBlocks.CQRS;
using Marten.Linq.QueryHandlers;

namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryQuery(string category) : IQuery<GetPrdouctByCategoryResult>;
    public record GetPrdouctByCategoryResult(IEnumerable<Product> proudct);

    public class GetProductsByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductsByCategoryQueryHandler> logger) : IQueryHandler<GetProductByCategoryQuery, GetPrdouctByCategoryResult>
    {
        public async Task<GetPrdouctByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductsByCategoryQueryHandler information is {query}", query);
            var products = await session.Query<Product>().Where(p => p.Category.Contains(query.category)).ToListAsync();
            return new GetPrdouctByCategoryResult(products);          
        }
    }
}
         