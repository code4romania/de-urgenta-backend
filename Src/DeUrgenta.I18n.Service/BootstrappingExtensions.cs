using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeUrgenta.I18n.Service
{

    public static class BootstrappingExtensions
    {
        public static void SetupI18nService(this IServiceCollection services, IConfiguration configuration)
        {


        }

        public static IHealthChecksBuilder AddI18nService(this IHealthChecksBuilder builder, IConfiguration configuration)
        {

            return builder;
        }
    }
}
