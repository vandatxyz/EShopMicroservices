namespace Basket.API.Basket.GetBasket
{
    //public record GetBasketRequest(string UserName);

    public record GetBasketResponse(ShoppingCart Cart);
    public class GetBasketEndPoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
            {
                // Validate the request.
                var query = new GetBasketQuery(userName);
                // Send the query to the handler.
                var result = await sender.Send(query);
                // Map the result to the response.
                var response = result.Adapt<GetBasketResponse>();
                // Return the response with a 200 OK status code.
                return Results.Ok(response);
            })
            .WithName("GetBasket")
            .WithSummary("Get Basket")
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Get basket by user name")
            ;
        }
    }
}
