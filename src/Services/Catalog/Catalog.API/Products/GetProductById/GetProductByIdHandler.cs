
namespace Catalog.API.Products.GetProductById
{
    // Query to get a product by its ID.
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
    // Result containing the product.
    public record GetProductByIdResult(Product Product);

    internal class GetProductByIdQueryHandler
        (IDocumentSession session)
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery command, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException(command.Id);
            }
            return new GetProductByIdResult(product);
        }
    }
}
