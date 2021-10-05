using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.RecurringJobs.Domain;
using DeUrgenta.RecurringJobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.RecurringJobs.Jobs
{
    public class NotificationCleanupJob : INotificationCleanupJob
    {
        private readonly JobsContext _jobsContext;

        public NotificationCleanupJob(JobsContext jobsContext)
        {
            _jobsContext = jobsContext;
        }

        public async Task RunAsync()
        {
            var notificationsToDelete = await _jobsContext.Notifications
                .Where(n => n.Status == NotificationStatus.Sent)
                .ToListAsync();

            _jobsContext.Notifications.RemoveRange(notificationsToDelete);
            await _jobsContext.SaveChangesAsync();
        }
    }
}