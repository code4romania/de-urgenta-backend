using System.Globalization;
using System.Linq;
using DeUrgenta.Domain.I18n;
using DeUrgenta.I18n.Service.Providers;
using DeUrgenta.Infra.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
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

            var serviceProvider = services.BuildServiceProvider();
            var languageService = serviceProvider.GetRequiredService<IamI18nProvider>();
            var languages = languageService.GetLanguages();
            var cultures = languages.GetAwaiter().GetResult().Select(x => new CultureInfo(x.Culture)).ToArray();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var englishCulture = cultures.FirstOrDefault(x => x.Name == "en-US");
                options.DefaultRequestCulture = new RequestCulture(englishCulture?.Name ?? "en-US");

                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });
        }

        public static IApplicationBuilder ConfigureI18n(this IApplicationBuilder builder)
        {
            builder.UseRequestLocalization();

            return builder;
        }
    }
}
