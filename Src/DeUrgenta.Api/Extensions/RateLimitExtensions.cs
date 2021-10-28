using AspNetCoreRateLimit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;


namespace DeUrgenta.Api.Extensions
{
    public static class RateLimitExtensions
    {
        public static void AddRateLimit(this IServiceCollection services, IConfiguration configuration)
        {
            // needed for loading options from appsettings.json
            services.AddOptions();

            // needed to store rate limit counters and ip rules
            services.AddMemoryCache();

            //load general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));

            //load ip rules from appsettings.json
            services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));

            // inject counter and rules stores
            services.AddInMemoryRateLimiting();

            // configuration (resolvers, counter key builders)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }
    }
}
