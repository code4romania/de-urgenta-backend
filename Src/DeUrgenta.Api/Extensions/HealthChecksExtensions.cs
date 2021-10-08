using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DeUrgenta.Emailing.Service;

namespace DeUrgenta.Api.Extensions
{
    public static class HealthChecksExtensions
    {
        public static IServiceCollection SetupHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHealthChecks()
                .AddNpgSql(configuration.GetConnectionString("DbConnectionString"), name: "de-urgenta-db")
                .AddNpgSql(configuration.GetConnectionString("IdentityDbConnectionString"), name: "identity-db")
                .AddEmailingService(configuration);

            services
                .AddHealthChecksUI()
                .AddInMemoryStorage();

            return services;
        }
    }
}
