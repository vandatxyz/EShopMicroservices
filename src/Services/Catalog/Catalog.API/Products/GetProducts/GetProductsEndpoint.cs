
namespace Catalog.API.Products.GetProducts
{
    public record GetProductsReponse(IEnumerable<Product> Products);

    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (ISender sender) =>
            {
                // Send the query to the handler.
                var result = await sender.Send(new GetProductQuery());
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
