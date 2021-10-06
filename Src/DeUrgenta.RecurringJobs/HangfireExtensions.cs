using DeUrgenta.RecurringJobs.Jobs;
using DeUrgenta.RecurringJobs.Jobs.Config;
using DeUrgenta.RecurringJobs.Services;
using DeUrgenta.RecurringJobs.Services.NotificationSenders;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeUrgenta.RecurringJobs
{
    public static class HangfireExtensions
    {
        public static void AddHangfireServices(this IServiceCollection services)
        {
            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMemoryStorage()
                );

            // Add the processing server as IHostedService
            services.AddHangfireServer();
        }

        public static void AddRecurringJobs(this IServiceCollection services, IConfiguration configuration)
        {
            var notificationSendersConfig = configuration.GetSection("NotificationSenders")
                .Get<NotificationSendersConfig>();

            if (notificationSendersConfig.NoOpNotificationSenderEnabled)
            {
                services.AddScoped<INotificationSender, NoOpNotificationSender>();
            }
            if (notificationSendersConfig.EmailNotificationSenderEnabled)
            {
                services.AddScoped<INotificationSender, EmailNotificationSender>();
            }
            if (notificationSendersConfig.PushNotificationSenderEnabled)
            {
                services.AddScoped<INotificationSender, PushNotificationSender>();
            }

            services.AddScoped<INotificationService, NotificationService>();

            services.Configure<EventArchivalJobConfig>(configuration.GetSection("RecurringJobsConfig:EventArchivalJobConfig"));
            services.AddScoped<IEventArchivalJob, EventArchivalJob>();

            services.Configure<ExpiredCertificationJobConfig>(configuration.GetSection("RecurringJobsConfig:ExpiredCertificationJobConfig"));
            services.AddScoped<IExpiredCertificationJob, ExpiredCertificationJob>();

            services.Configure<NotificationSenderJobConfig>(configuration.GetSection("RecurringJobsConfig:NotificationSenderJobConfig"));
            services.AddScoped<INotificationSenderJob, NotificationSenderJob>();

            services.Configure<NotificationCleanupJobConfig>(configuration.GetSection("RecurringJobsConfig:NotificationCleanupJobConfig"));
            services.AddScoped<INotificationCleanupJob, NotificationCleanupJob>();
        }

        public static void UseAuthenticatedHangfireDashboard(this IApplicationBuilder app, IConfiguration configuration)
        {
            var filter = new BasicAuthAuthorizationFilter(
                        new BasicAuthAuthorizationFilterOptions
                        {
                            // Require secure connection for dashboard
                            RequireSsl = false,
                            SslRedirect = false,
                            // Case sensitive login checking
                            LoginCaseSensitive = true,
                            // Users
                            Users = new[]
                            {
                                new BasicAuthAuthorizationUser
                                {
                                    Login = configuration["Hangfire:Dashboard:Username"],
                                    // Password as plain text, SHA1 will be used
                                    PasswordClear = configuration["Hangfire:Dashboard:Password"]
                                }
                            }
                        });

            var options = new DashboardOptions
            {
                DashboardTitle = "De Urgenta Hangfire Dashboard",
                Authorization = new IDashboardAuthorizationFilter[]
                {
                    filter
                }
            };
            app.UseHangfireDashboard(options: options);
        }
    }
}
