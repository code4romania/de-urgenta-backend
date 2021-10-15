using System;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.RecurringJobs.Domain;
using DeUrgenta.RecurringJobs.Domain.Entities;
using DeUrgenta.RecurringJobs.Jobs.Config;
using DeUrgenta.RecurringJobs.Jobs.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.RecurringJobs.Jobs
{
    public class ExpiredCertificationJob : IExpiredCertificationJob
    {
        private readonly DeUrgentaContext _context;
        private readonly JobsContext _jobsContext;
        private readonly ExpiredCertificationJobConfig _config;

        public ExpiredCertificationJob(DeUrgentaContext context, JobsContext jobsContext, IOptionsMonitor<ExpiredCertificationJobConfig> config)
        {
            _context = context;
            _jobsContext = jobsContext;
            _config = config.CurrentValue;
        }

        public async Task RunAsync()
        {
            var expiringCertifications = await _context.Certifications
                .Where(c => (c.ExpirationDate - DateTime.Today).Days <= _config.DaysBeforeExpirationDate
                                    && c.ExpirationDate >= DateTime.Today)
                .ToListAsync();

            foreach (var expiringCertification in expiringCertifications)
            {
                var scheduledNotifications = await _jobsContext.Notifications
                    .Where(n => n.CertificationDetails.CertificationId == expiringCertification.Id
                                        && n.ScheduledDate >= DateTime.Today)
                    .Select(n => n.CertificationDetails.CertificationId)
                    .ToListAsync();

                if (scheduledNotifications.Count != 0)
                {
                    continue;
                }

                var computedNotificationDate = expiringCertification.ExpirationDate.AddDays(-_config.DaysBeforeExpirationDate);
                var preExpirationNotification = new Notification
                {
                    Type = NotificationType.Certification,
                    ScheduledDate = computedNotificationDate < DateTime.Today ? DateTime.Today : computedNotificationDate,
                    CertificationDetails = new CertificationDetails { CertificationId = expiringCertification.Id },
                    UserId = expiringCertification.UserId,
                    Status = NotificationStatus.NotSent
                };
                var expirationDayNotification = new Notification
                {
                    Type = NotificationType.Certification,
                    ScheduledDate = expiringCertification.ExpirationDate,
                    CertificationDetails = new CertificationDetails { CertificationId = expiringCertification.Id },
                    UserId = expiringCertification.UserId,
                    Status = NotificationStatus.NotSent
                };

                await _jobsContext.AddAsync(preExpirationNotification);
                await _jobsContext.AddAsync(expirationDayNotification);
                await _jobsContext.SaveChangesAsync();
            }
        }
    }
}
