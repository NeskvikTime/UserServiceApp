using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UserServiceApp.Application.Common.Interfaces;

namespace UserServiceApp.Infrastructure.DbInitializer
{
    public static class MigrationsExtension
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

            dbContext.Migrate();
        }
    }
}
