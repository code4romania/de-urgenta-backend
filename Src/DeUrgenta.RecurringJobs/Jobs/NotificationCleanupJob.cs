using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Domain.RecurringJobs;
using DeUrgenta.Domain.RecurringJobs.Entities;
using DeUrgenta.RecurringJobs.Jobs.Interfaces;
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

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var notificationsToDelete = await _jobsContext.Notifications
                .Where(n => n.Status == NotificationStatus.Sent)
                .ToListAsync(cancellationToken: cancellationToken);

            _jobsContext.Notifications.RemoveRange(notificationsToDelete);
            await _jobsContext.SaveChangesAsync(cancellationToken);
        }
    }
}