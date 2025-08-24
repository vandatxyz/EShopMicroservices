
namespace Catalog.API.Products.GetProductById
{
    // Query to get a product by its ID.
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
    // Result containing the product.
    public record GetProductByIdResult(Product Product);

    internal class GetProductByIdQueryHandler
        (IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger)
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery command, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductByIdQueryHandler.Handle called with {@Query}", command);

            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException();
            }
            return new GetProductByIdResult(product);
        }
    }
}
