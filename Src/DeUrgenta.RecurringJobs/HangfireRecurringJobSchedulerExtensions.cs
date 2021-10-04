﻿using System;
using DeUrgenta.RecurringJobs.Jobs;
using DeUrgenta.RecurringJobs.Jobs.Config;
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
        }
    }
}
