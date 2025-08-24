namespace Catalog.API.Products.GetProductByCategory
{

    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

    public record GetProductByCategoryResult(IEnumerable<Product> Products);
    internal class GetProductByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductByCategoryQueryHandler> logger)
        : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            // Log the incoming query for debugging purposes
            logger.LogInformation("GetProductByCategoryQueryHandler.Handle called with {@Query}", query);
            // Query products by category using a case-insensitive contains search
            var products = await session.Query<Product>()
                .Where(p => p.Category.Contains(query.Category))
                .ToListAsync(cancellationToken);
            // If no products found, return empty list
            return new GetProductByCategoryResult(products);
        }
    }
}
