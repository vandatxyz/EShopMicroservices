

using Discount.Grpc;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container before building application.



// Add Carter for building modular APIs and MediatR for CQRS pattern.
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// Register all validators from the assembly for MediatR commands and queries. marten
builder.Services.AddMarten(options =>
{
    // Configure the database connection string for Marten.
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    // Define the document schema for the ShoppingCart entity, using UserName as the identity field.
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions(); // Use lightweight sessions for Marten, which are suitable for most scenarios.

// In development environment, initialize Marten with initial data.
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

// Add distributed caching using Redis.
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

// Configure Redis cache settings.
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    //options.InstanceName = "Basket_";
});

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(option =>
{
    option.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler
    {
        // Return `true` to allow certificates that are untrusted/invalid
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
    return handler;
});
// Register all validators from the assembly for MediatR commands and queries.
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// Add health checks to monitor the health of the application and its dependencies.
//cross-cutting concerns such as logging, validation, and exception handling.
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!)
    ;


var app = builder.Build();

// Configure the HTTP request pipeline after build application.
app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();
