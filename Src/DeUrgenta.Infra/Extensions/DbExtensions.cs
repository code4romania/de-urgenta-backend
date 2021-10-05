using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DeUrgenta.Infra.Extensions
{
    public static class DbExtensions
    {
        public static IServiceCollection AddDatabase<T>(this IServiceCollection services, string connectionString) where T : DbContext
        {
              services.AddDbContextPool<T>(options =>
                options.UseNpgsql(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null
                    )));

            return services;
        }

        public static void UseDatabase<T>(this IServiceProvider serviceProvider) where T:DbContext
        {
            using var scope = serviceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<T>().CreateAndMigrate();
        }

        public static void CreateAndMigrate(this DbContext context)
        {
            context.Database.Migrate();
        }
    }
}
