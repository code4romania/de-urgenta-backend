using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeUrgenta.Infra.Extensions
{
    public static class OptionsExtensions
    {
        public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(sectionName);
            var options = new T();
            section.Bind(options);

            return options;
        }

        public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
        {
            var section = configuration.GetSection(sectionName);
            var options = new T();
            section.Bind(options);

            return options;
        }

        public static IServiceCollection ConfigureOptions<T>(this IServiceCollection services, string sectionName) where T : class
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            services.Configure<T>(configuration.GetSection(sectionName));

            return services;
        }
    }
}
