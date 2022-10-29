using System;
using System.Threading;
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
                    job => job.RunAsync(CancellationToken.None),
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
                    job => job.RunAsync(CancellationToken.None),
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
                    job => job.RunAsync(CancellationToken.None),
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
                    job => job.RunAsync(CancellationToken.None),
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
                    job => job.RunAsync(CancellationToken.None),
                    expiredBackpackItemJobConfig.CronExpression,
                    TimeZoneInfo.Utc
                );
            }

            var expiredInviteJobConfig = configuration.GetSection("RecurringJobsConfig:ExpiredInviteJobConfig")
                .Get<ExpiredInviteJobConfig>();
            if (expiredInviteJobConfig.IsEnabled)
            {
                RecurringJob.AddOrUpdate<IExpiredInviteJob>(
                    nameof(ExpiredInviteJob),
                    job => job.RunAsync(CancellationToken.None),
                    expiredInviteJobConfig.CronExpression,
                    TimeZoneInfo.Utc
                    );
            }

            var acceptedInviteJobConfig = configuration.GetSection("RecurringJobsConfig:AcceptedInviteJobConfig")
                .Get<AcceptedInviteJobConfig>();
            if (acceptedInviteJobConfig.IsEnabled)
            {
                RecurringJob.AddOrUpdate<IAcceptedInviteJob>(
                    nameof(AcceptedInviteJob),
                    job => job.RunAsync(CancellationToken.None),
                    expiredInviteJobConfig.CronExpression,
                    TimeZoneInfo.Utc
                );
            }
        }
    }
}
