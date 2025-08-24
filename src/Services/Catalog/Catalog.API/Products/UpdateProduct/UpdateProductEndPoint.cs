namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductRequest(Guid Id, string Name, decimal Price, string Description, List<string> Category, string ImageFile);
    public record UpdateProductResponse(bool IsSuccess);
    public class UpdateProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products",
            async (ISender sender, UpdateProductRequest request) =>
            {
                // Map the incoming request to the command
                var command = request.Adapt<UpdateProductCommand>();
                // Send the command to the handler
                var result = await sender.Send(command);
                // Mapster the result to the response
                var response = result.Adapt<UpdateProductResponse>();
                // Return the response with a 200 OK status code
                return Results.Ok(response);
            })
            .WithName("UpdateProduct")
            .WithSummary("Update Product")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithDescription("Update an existing product")
            ;
        }
    }
}
