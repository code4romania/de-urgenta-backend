using System;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DeUrgenta.Infra.Extensions
{
    public static class DbExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<DeUrgentaContext>(options =>
                options.UseNpgsql(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null
                    )));

            return services;
        }
    }
}
