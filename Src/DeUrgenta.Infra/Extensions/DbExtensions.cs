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

            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<T>();
            dbContext.Database.Migrate();

            return services;
        }
    }
}
