
namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketRequest(ShoppingCart Cart);

    public record StoreBasketResponse(string UserName);

    public class StoreBasketEndPoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
            {
                // Validate the request.
                var command = request.Adapt<StoreBasketCommand>();
                // Send the command to the handler.
                var result = await sender.Send(command);
                // Map the result to the response.
                var response = result.Adapt<StoreBasketResponse>();
                // Return the response with a 201 Created status code.
                return Results.Created($"/basket/{response.UserName}", response);
            })
            .WithDescription("Store a shopping cart for a user")
            .WithName("StoreBasket")
            .WithSummary("Store Basket")
            .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            ;
        }
    }
}
