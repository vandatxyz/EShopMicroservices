using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public static class Extensions
    {
        // This method ensures that the database is created and applies any pending migrations.
        public static IApplicationBuilder UseMigration(this IApplicationBuilder app) 
        {
            // Create a new scope to get the required services.
            using var scope = app.ApplicationServices.CreateScope();
            // Get the service provider from the scope.
            var services = scope.ServiceProvider;
            // Get the DiscountContext from the service provider.
            var context = services.GetRequiredService<DiscountContext>();
            // Apply any pending migrations to the database.
            context.Database.MigrateAsync();
            // Return the application builder for further configuration.
            return app;
        }
    }
}
