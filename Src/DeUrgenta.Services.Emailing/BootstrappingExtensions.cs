using DeUrgenta.Emailing.Service.Builders;
using DeUrgenta.Emailing.Service.Config;
using DeUrgenta.Emailing.Service.Senders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeUrgenta.Emailing.Service
{
    public static class BootstrappingExtensions
    {
        public static void SetupEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailOptions>(configuration.GetSection("Email:Options"));
            services.AddTransient<IEmailBuilderService, EmailBuilderService>();

            var emailType = configuration.GetValue<EmailingSystemTypes>("Email:EmailingSystem");
            
            switch (emailType)
            {
                case EmailingSystemTypes.SendGrid:
                    services.Configure<SendGridOptions>(configuration.GetSection("Email:SendGrid"));
                    services.AddSingleton<IEmailSender, SendGridSender>();
                    break;

                case EmailingSystemTypes.Smtp:
                    services.Configure<SmtpOptions>(configuration.GetSection("Email:Smtp"));
                    services.AddSingleton<IEmailSender, SmtpSender>();
                    break;
            }
        }

    }
}
