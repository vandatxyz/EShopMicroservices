using Marten.Schema;

namespace Catalog.API.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();
            // Add any initial data population logic here if needed.
            if(await session.Query<Product>().AnyAsync(cancellation))
            {
                return;
            }

            // Marten Upsert will carter for existing records
            session.Store<Product>(GetPreconfiguredProducts());
            await session.SaveChangesAsync(cancellation);
        }

        private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>()
        {
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "IPhone X",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                Price = 950.00M,
                Category = ["Smart Phone"],
                ImageFile = "product-1.png"
            },
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Samsung 10",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                Price = 840.00M,
                Category = ["Smart Phone"],
                ImageFile = "product-2.png"
            },
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Huawei Plus",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                Price = 650.00M,
                Category = ["White Appliances"],
                ImageFile = "product-3.png"
            },
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Xiaomi Mi 9",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                Price = 470.00M,
                Category = ["White Appliances"],
                ImageFile = "product-4.png"
            },
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "HTC U11+ Plus",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                Price = 380.00M,
                Category = ["Smart Phone"],
                ImageFile = "product-5.png"
            },
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "LG G7 ThinQ",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                Price = 240.00M,
                Category = ["Home Kitchen"],
                ImageFile = "product-6.png"
            },
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Dell XPS 13",
                Description = "The Dell XPS 13 is a high-performance laptop with a sleek design, featuring a nearly borderless display, powerful processors, and long battery life, making it ideal for both work and entertainment on the go.",
                Price = 1200.00M,
                Category = ["Computers"],
                ImageFile = "product-7.png"
            }
        };
    }
}
