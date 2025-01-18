using Marten.Schema;

namespace Catalog.API.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            var session =store.LightweightSession();
            if (await session.Query<Product>().AnyAsync()) return;
            session.Store<Product>(GetPreConfiguredProducts());
            await session.SaveChangesAsync();

        }
        private static IEnumerable<Product> GetPreConfiguredProducts() => new List<Product>
        {
            {
                new Product()
                {
                    Id=new Guid(),
                    Name="IPhone X",
                    Description="Tnis Phone is company biggest change",
                    Price=10.99M,
                    Category=new List<string>{"Smart Phone"}
                }
            }
        };
    }

}
