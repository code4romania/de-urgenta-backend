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
            var emailType = configuration.GetValue<EmailingSystemTypes>("EMailingSystem");

            switch (emailType)
            {
                case EmailingSystemTypes.SendGrid:
                    services.AddSingleton(new SendGridOptions
                    {
                        ApiKey = configuration["SendGrid:ApiKey"],
                        ClickTracking = configuration.GetValue<bool>("SendGrid:ClickTracking")
                    });

                    services.AddSingleton<IEmailSender, SendGridSender>();
                    break;

                case EmailingSystemTypes.Smtp:
                    services.AddSingleton(new SmtpOptions
                    {
                        Host = configuration["Smtp:Host"],
                        Port = configuration.GetValue<int>("Smtp:Port"),
                        User = configuration["Smtp:User"],
                        Password = configuration["Smtp:Password"]
                    });

                    services.AddSingleton<IEmailSender, SmtpSender>();
                    break;
            }
        }

    }
}
