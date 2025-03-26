using E_commerce_Api.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Extensions;

public static class ApplicationBuilderExtensions
{
    internal static void ApplyMigrations(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();

        using var applicationDbContext =
            scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();

        //applicationDbContext.Database.Migrate();
    }
}