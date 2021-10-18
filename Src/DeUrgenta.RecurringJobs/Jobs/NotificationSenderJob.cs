using System;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.RecurringJobs.Jobs.Interfaces;
using DeUrgenta.RecurringJobs.Services;
using Microsoft.EntityFrameworkCore;
using DeUrgenta.Domain.RecurringJobs;
using DeUrgenta.Domain.RecurringJobs.Entities;
using DeUrgenta.RecurringJobs.Jobs.Config;
using Microsoft.Extensions.Options;

namespace DeUrgenta.RecurringJobs.Jobs
{
    public class NotificationSenderJob : INotificationSenderJob
    {
        private readonly INotificationService _notificationService;
        private readonly JobsContext _jobsContext;
        private readonly NotificationSenderJobConfig _config;

        public NotificationSenderJob(INotificationService notificationService, JobsContext jobsContext, IOptions<NotificationSenderJobConfig> config)
        {
            _notificationService = notificationService;
            _jobsContext = jobsContext;
            _config = config.Value;
        }

        public async Task RunAsync()
        {
            var notificationsToSend = await _jobsContext.Notifications
                .Where(n => n.Status == NotificationStatus.NotSent
                                    && n.ScheduledDate.Date == DateTime.Today)
                .Take(_config.BatchSize)
                .ToListAsync();

            foreach (var notification in notificationsToSend)
            {
                var notificationStatus = await _notificationService.SendNotificationAsync(notification.Id);

                notification.Status = notificationStatus;
                await _jobsContext.SaveChangesAsync();
            }
        }
    }
}
