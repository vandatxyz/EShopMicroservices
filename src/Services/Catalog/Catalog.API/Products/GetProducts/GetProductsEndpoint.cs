
namespace Catalog.API.Products.GetProducts
{

    public record GetProductRequest(int? PageNumber = 1, int? PageSize = 10);

    public record GetProductsReponse(IEnumerable<Product> Products);

    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (ISender sender, [AsParameters] GetProductRequest request) =>
            {
                // Validate the request.
                var query = request.Adapt<GetProductQuery>();
                // Send the query to the handler.
                var result = await sender.Send(query);
                // Map the result to the response.
                var response = result.Adapt<GetProductsReponse>();
                // Return the response with a 200 OK status code.
                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .WithSummary("Get Products")
            .Produces<GetProductsReponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Get all products")
            ;
        }
    }
}
