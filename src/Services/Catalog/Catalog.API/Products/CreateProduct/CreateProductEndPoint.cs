namespace Catalog.API.Products.CreateProduct
{
    public record CreateRecordRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);
    public record CreateResponse(Guid Id);

    public class CreateProductEndPoint
    {
    }
}
