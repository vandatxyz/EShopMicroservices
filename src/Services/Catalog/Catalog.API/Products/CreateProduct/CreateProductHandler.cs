using FluentValidation;

namespace Catalog.API.Products.CreateProduct
{
    // This record represents the command to create a new product.
    public record CreateProductCommand(string Name, decimal Price, string Description, List<string> Category, string ImageFile)
        : ICommand<CreateProductResult>;

    // This record represents the result of creating a product, containing the product ID.
    public record CreateProductResult(Guid Id);

    // Validator for the CreateProductCommand to ensure data integrity.
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.Category).NotEmpty().WithMessage("At least one category is required.");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image file is required.");
        }
    }

    internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
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

            //TODO: Add domain events, validations, etc.
            // save it to the database.
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            // return the result with the product ID.
            return new CreateProductResult(product.Id);
        }
    }
}
