using BuildingBlocks.CQRS;
using Catalog.API.Models;
using MediatR;

namespace Catalog.API.Products.CreateProduct
{
    // This record represents the command to create a new product.
    public record CreateProductCommand(string Name, decimal Price, string Description, List<string> Category, string ImageFile)
        : ICommand<CreateProductResult>;

    // This record represents the result of creating a product, containing the product ID.
    public record CreateProductResult(Guid Id);

    internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        // This is where you would typically inject your DbContext or repository to interact with the database.
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // create the product.
            var product = new Product
            {
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Name = command.Name,
                Price = command.Price,
            };
            // save it to the database.


            // return the result with the product ID.
            return new CreateProductResult(Guid.NewGuid());
        }
    }
}
