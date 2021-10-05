using DeUrgenta.Services.Emailing.Config;
using DeUrgenta.Services.Emailing.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeUrgenta.Services.Emailing
{
    public static class BootstrappingExtensions
    {
        public static void SetupEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEmailBuilderService, EmailBuilderService>();
            services.AddSingleton<ITemplateFileSelector, TemplateFileSelector>();

            var emailType = configuration.GetValue<EmailingSystemTypes>("EmailingSystem");

            switch (emailType)
            {
                case EmailingSystemTypes.SendGrid:
                    services.Configure<SendGridOptions>(configuration.GetSection("SendGrid"));
                    services.AddSingleton<IEmailSender, SendGridSender>();
                    break;

                case EmailingSystemTypes.Smtp:
                    services.Configure<SmtpOptions>(configuration.GetSection("Smtp"));
                    services.AddSingleton<IEmailSender, SmtpSender>();
                    break;
            }
        }

    }
}
