using DeUrgenta.Emailing.Service.Builders;
using DeUrgenta.Emailing.Service.Config;
using DeUrgenta.Emailing.Service.Senders;
using DeUrgenta.Infra.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeUrgenta.Emailing.Service
{

    public static class BootstrappingExtensions
    {
        public static void SetupEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            services.Configure<EmailConfigOptions>(configuration.GetSection("Email:Config"));
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

        public static IHealthChecksBuilder AddEmailingService(this IHealthChecksBuilder builder, IConfiguration configuration)
        {
            var checksEnabled = configuration.GetValue<bool>("Email:EnableHealthChecks");

            if (!checksEnabled)
            {
                return builder;
            }

            var emailType = configuration.GetValue<EmailingSystemTypes>("Email:EmailingSystem");

            switch (emailType)
            {
                case EmailingSystemTypes.SendGrid:
                    var sendGridOptions = configuration.GetOptions<SendGridOptions>("Email:SendGrid");

                    builder.AddSendGrid(sendGridOptions.ApiKey);
                    break;

                case EmailingSystemTypes.Smtp:
                    var smtpOptions = configuration.GetOptions<SmtpOptions>("Email:Smtp");

                    builder.AddSmtpHealthCheck(setup =>
                    {
                        setup.Host = smtpOptions.Host;
                        setup.Port = smtpOptions.Port;
                        setup.LoginWith(smtpOptions.User, smtpOptions.Password);
                    });

                    break;
            }

            return builder;
        }
    }
}
