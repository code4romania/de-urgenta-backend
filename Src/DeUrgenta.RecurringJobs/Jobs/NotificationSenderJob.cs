using System;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.RecurringJobs.Domain;
using DeUrgenta.RecurringJobs.Domain.Entities;
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
            var notificationIdsToSend = await _jobsContext.Notifications
                .Where(n => n.Status == NotificationStatus.NotSent 
                                    && n.ScheduledDate.Date == DateTime.Today)
                .Select(n => n.Id)
                .ToListAsync();

            foreach (var notificationId in notificationIdsToSend)
            {
                await _notificationService.SendNotificationAsync(notificationId);
            }
        }
    }
}
