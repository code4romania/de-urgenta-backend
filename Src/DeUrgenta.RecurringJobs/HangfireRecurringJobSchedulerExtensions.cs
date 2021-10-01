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
                RecurringJob.AddOrUpdate<ExpiredCertificationJob>(
                    nameof(ExpiredCertificationJob),
                    job => job.RunAsync(),
                    expiredCertificationJobConfig.CronExpression,
                    TimeZoneInfo.Utc
                );
            }
        }
    }
}