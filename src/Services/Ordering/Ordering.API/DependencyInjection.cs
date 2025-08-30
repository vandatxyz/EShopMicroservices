namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            // Add API services here, e.g., Controllers, Swagger, CORS, etc.
            //services.AddCarter();

            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            // Configure the HTTP request pipeline here, e.g., Middleware, Endpoints, etc.

            //app.MapCarter();
            return app;
        }
    }
}
