
using Microsoft.AspNetCore.Session;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, decimal Price, int Stock) : ICommand<UpdateProductResult> ;
    public record UpdateProductResult(bool IsSuccess);
    internal  class UpdateProducCommandtHandler(IDocumentSession session,ILogger<UpdateProducCommandtHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation( "UpdateProductHandler class call with Command {command}",command);
           var product= await session.LoadAsync<Product>(command.Id, cancellationToken);
            if (product is null)
                throw new ProductNotFoundException();
            product.Name = command.Name;
            product.Description = command.Description;
            product.Price = command.Price;
            product.Category = command.Category;
            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);
            return new UpdateProductResult(true);

        }
    }
}
       