namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdReponse(Product Product);

    public class GetProductByIdEndPoint : ICarterModule

           {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id:guid}", async (ISender sender, Guid id) =>
            {
                // Send the query to the handler.
                var result = await sender.Send(new GetProductByIdQuery(id));
                // Map the result to the response.
                var response = result.Adapt<GetProductByIdReponse>();
                // Return the response with a 200 OK status code.
                return Results.Ok(response);
            })
            .WithName("GetProductById")
            .WithSummary("Get Product By Id")
            .Produces<GetProductByIdReponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Get product by id")
            ;
        }
    }
}
