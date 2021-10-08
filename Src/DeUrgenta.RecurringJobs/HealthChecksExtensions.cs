using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DeUrgenta.Emailing.Service;

namespace DeUrgenta.RecurringJobs
{
    public static class HealthChecksExtensions
    {
        public static IServiceCollection SetupHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHealthChecks()
                .AddNpgSql(configuration.GetConnectionString("DbConnectionString"), name: "de-urgenta-db")
                .AddNpgSql(configuration.GetConnectionString("JobsConnectionString"), name: "jobs-db")
                .AddEmailingService(configuration);

            services
                .AddHealthChecksUI()
                .AddInMemoryStorage();

            return services;
        }
    }
}