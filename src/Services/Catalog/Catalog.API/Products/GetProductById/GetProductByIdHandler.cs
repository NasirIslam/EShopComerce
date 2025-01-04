using BuildingBlocks.CQRS;
using Catalog.API.Products.GetProducts;

namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Product Product);
    internal class GetProductByIdQueryHandler(IDocumentSession _session, ILogger<GetProductByIdQueryHandler> _logger) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetProductByIdQueryHandler entering in the class {Session} ", _session);
            var product = await _session.LoadAsync<Product>(query.Id, cancellationToken);
            if (product is null)
                throw new ProductNotFoundException();
            return new GetProductByIdResult(product);
           
        }
    }
}
