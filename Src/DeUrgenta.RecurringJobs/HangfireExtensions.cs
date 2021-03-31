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

        public static void UseAuthenticatedHangfireDashboard(this IApplicationBuilder app, IConfiguration configuration)
        {
            var filter = new BasicAuthAuthorizationFilter(
                        new BasicAuthAuthorizationFilterOptions
                        {
                            // Require secure connection for dashboard
                            RequireSsl = true,
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
