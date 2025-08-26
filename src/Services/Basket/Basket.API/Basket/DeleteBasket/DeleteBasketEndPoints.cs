
namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketRequest(string UserName);

    public record DeleteBasketReponse(bool IsSuccess);

    public class DeleteBasketEndPoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{UserName}", async (string UserName, ISender sender) =>
            {
                var result = await sender.Send(new DeleteBasketCommand(UserName));
                var response = result.Adapt<DeleteBasketReponse>();
                return Results.Ok(response);
            })
            .WithName("DeleteBasket")
            .WithSummary("Delete Basket")
            .Produces<DeleteBasketReponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Delete basket by user name")
            ;
        }
    }
}
