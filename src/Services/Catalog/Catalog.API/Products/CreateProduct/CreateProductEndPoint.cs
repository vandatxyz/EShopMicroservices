namespace Catalog.API.Products.CreateProduct
{

    public record CreateProductRequest(string Name, decimal Price, string Description, List<string> Category, string ImageFile);

    public record CreateProductResponse(Guid Id);

    public class CreateProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                // Validate the request.
                var command = request.Adapt<CreateProductCommand>();
                // Send the command to the handler.
                var result = await sender.Send(command);
                // Map the result to the response.
                var response = result.Adapt<CreateProductResponse>();
                // Return the response with a 201 Created status code.
                return Results.Created($"/products/{response.Id}", response);
            })
            // Map the endpoint to the CreateProductCommand
            .WithName("CreateProduct")
            .WithSummary("Create Product")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Create a new product")
            ;
        }
    }
}
