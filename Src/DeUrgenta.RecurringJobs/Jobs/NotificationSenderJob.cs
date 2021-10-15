using System;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.RecurringJobs.Domain;
using DeUrgenta.RecurringJobs.Domain.Entities;
using DeUrgenta.RecurringJobs.Jobs.Interfaces;
using DeUrgenta.RecurringJobs.Services;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.RecurringJobs.Jobs
{
    public class NotificationSenderJob : INotificationSenderJob
    {
        private readonly INotificationService _notificationService;
        private readonly JobsContext _jobsContext;

        public NotificationSenderJob(INotificationService notificationService, JobsContext jobsContext)
        {
            _notificationService = notificationService;
            _jobsContext = jobsContext;
        }

        public async Task RunAsync()
        {
            var notificationsToSend = await _jobsContext.Notifications
                .Where(n => n.Status == NotificationStatus.NotSent 
                                    && n.ScheduledDate.Date == DateTime.Today)
                .ToListAsync();

            foreach (var notification in notificationsToSend)
            {
                await _notificationService.SendNotificationAsync(notification.Id);

                notification.Status = NotificationStatus.Sent;
                await _jobsContext.SaveChangesAsync();
            }
        }
    }
}
