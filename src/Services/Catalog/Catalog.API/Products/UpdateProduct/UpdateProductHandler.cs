
using Microsoft.AspNetCore.Session;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, decimal Price, int Stock) : ICommand<UpdateProductResult> ;
    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandValidator:AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator() {
            RuleFor(c => c.Id).NotEmpty().WithMessage("Id is mandatory");
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name is mandatory").Length(2,100).WithMessage("Length should be greater than 2 and less than 100");
            RuleFor(c => c.Price).GreaterThan(0).WithMessage("Price should be greater than zero");
        }
        
    }
    internal  class UpdateProducCommandtHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
          
           var product= await session.LoadAsync<Product>(command.Id, cancellationToken);
            if (product is null)
                throw new ProductNotFoundException(command.Id);
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
       