

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
{
    // Initialize Marten with initial data in development environment
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

// Add custom exception handler
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();

// Use the custom exception handler middleware
app.UseExceptionHandler(options => { });

#region Notused

//// Global Exception Handler
//app.UseExceptionHandler(exceptionHandlerApp =>
//{
//    // This middleware will catch exceptions thrown in the application
//    exceptionHandlerApp.Run(async context =>
//    {
//        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
//        if (exception is null)
//        {
//            return;
//        }
//        // Log the exception and return a generic error response
//        var problemDetails = new ProblemDetails
//        {
//            Status = StatusCodes.Status500InternalServerError,
//            Title = "An unexpected error occurred!",
//            Detail = exception.Message
//        };
//        // Log the exception
//        var logger = app.Services.GetRequiredService<ILogger<Program>>();
//        logger.LogError(exception, exception.Message);

//        // Return the ProblemDetails response
//        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//        context.Response.ContentType = "application/problem+json";
//        await context.Response.WriteAsJsonAsync(problemDetails);
//    });
//});

#endregion

app.Run();
