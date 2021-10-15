using System.Linq;
using DeUrgenta.Domain.I18n;
using DeUrgenta.I18n.Service.Providers;
using DeUrgenta.Infra.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeUrgenta.I18n.Service
{
    public static class BootstrappingExtensions
    {
        public static void SetupI18nService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase<I18nDbContext>(configuration.GetConnectionString("I18nDbConnectionString"));
            services.AddLocalization();

            services.AddScoped<IamI18nProvider, I18NProvider>();
        }

        public static IApplicationBuilder ConfigureI18n(this IApplicationBuilder builder)
        {
            var serviceProvider = builder.ApplicationServices;

            using var scope = serviceProvider.CreateScope();
            var languageProvider = scope.ServiceProvider.GetRequiredService<IamI18nProvider>();
            var languages = languageProvider.GetLanguages();

            var supportedCultures = languages
                .GetAwaiter().GetResult()
                .Select(x => x.Culture)
                .ToArray();

            var englishCulture = supportedCultures.FirstOrDefault(x => x == "en-US") ?? "en-US";

            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(englishCulture)
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            builder.UseRequestLocalization(localizationOptions);

            return builder;
        }
    }
}
