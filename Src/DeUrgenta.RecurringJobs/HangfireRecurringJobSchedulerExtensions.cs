using System;
using DeUrgenta.RecurringJobs.Jobs;
using DeUrgenta.RecurringJobs.Jobs.Config;
using DeUrgenta.RecurringJobs.Jobs.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace DeUrgenta.RecurringJobs
{
    public static class HangfireRecurringJobSchedulerExtensions
    {
        public static void ScheduleJobs(this IApplicationBuilder app, IConfiguration configuration)
        {
            var expiredCertificationJobConfig = configuration.GetSection("RecurringJobsConfig:ExpiredCertificationJobConfig")
                .Get<ExpiredCertificationJobConfig>();
            if (expiredCertificationJobConfig.IsEnabled)
            {
                RecurringJob.AddOrUpdate<IExpiredCertificationJob>(
                    nameof(ExpiredCertificationJob),
                    job => job.RunAsync(),
                    expiredCertificationJobConfig.CronExpression,
                    TimeZoneInfo.Utc
                );
            }

            var sendNotificationsJob = configuration.GetSection("RecurringJobsConfig:NotificationSenderJobConfig")
                .Get<NotificationSenderJobConfig>();
            if (sendNotificationsJob.IsEnabled)
            {
                RecurringJob.AddOrUpdate<INotificationSenderJob>(
                    nameof(NotificationSenderJob),
                    job => job.RunAsync(),
                    sendNotificationsJob.CronExpression,
                    TimeZoneInfo.Utc
                );
            }

            var cleanupNotificationsJob = configuration.GetSection("RecurringJobsConfig:NotificationCleanupJobConfig")
                    .Get<NotificationCleanupJobConfig>();
            if (cleanupNotificationsJob.IsEnabled)
            {
                RecurringJob.AddOrUpdate<INotificationCleanupJob>(
                    nameof(NotificationCleanupJob),
                    job => job.RunAsync(),
                    cleanupNotificationsJob.CronExpression,
                    TimeZoneInfo.Utc
                );
            }
            
            var eventArchivalJobConfig = configuration.GetSection("RecurringJobsConfig:EventArchivalJobConfig")
                .Get<EventArchivalJobConfig>();
            if (eventArchivalJobConfig.IsEnabled)
            {
                RecurringJob.AddOrUpdate<IEventArchivalJob>(
                    nameof(EventArchivalJob),
                    job => job.RunAsync(),
                    eventArchivalJobConfig.CronExpression,
                    TimeZoneInfo.Utc
                );
            }

            var expiredBackpackItemJobConfig = configuration.GetSection("RecurringJobsConfig:ExpiredBackpackItemJobConfig")
                .Get<ExpiredBackpackItemJobConfig>();
            if (expiredBackpackItemJobConfig.IsEnabled)
            {
                RecurringJob.AddOrUpdate<IExpiredBackpackItemJob>(
                    nameof(ExpiredBackpackItemJob),
                    job => job.RunAsync(),
                    expiredBackpackItemJobConfig.CronExpression,
                    TimeZoneInfo.Utc
                );
            }
        }
    }
}
