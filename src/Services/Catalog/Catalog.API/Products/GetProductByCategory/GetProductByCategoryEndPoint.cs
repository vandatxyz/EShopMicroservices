namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);
    public class GetProductByCategoryEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (ISender sender, string category) =>
            {
                // Send the query to the handler.
                var result = await sender.Send(new GetProductByCategoryQuery(category));
                // Map the result to the response.
                var response = result.Adapt<GetProductByCategoryResponse>();
                // Return the response with a 200 OK status code.
                return Results.Ok(response);
            })
            .WithName("GetProductByCategory")
            .WithSummary("Get Product By Category")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Get product by category")
            ;
        }
    }
}
