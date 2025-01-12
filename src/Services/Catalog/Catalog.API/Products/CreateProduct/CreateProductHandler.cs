namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    public class CraeteCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CraeteCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name can not be empty");
            RuleFor(c => c.Price).GreaterThan(0).WithMessage("Price must be greater than zero");
        }
    }
        internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
        {
            public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
            {
                //Create object entity from Command Object
                //Save to Database
                //return CreateProductResult object
                var product = new Product
                {
                    Name = command.Name,
                    Category = command.Category,
                    Description = command.Description,
                    ImageFile = command.ImageFile,
                    Price = command.Price
                };
                //save to the database
                session.Store(product);
                await session.SaveChangesAsync(cancellationToken);
                //return the result
                return new CreateProductResult(product.Id);

            }
        }
    }
