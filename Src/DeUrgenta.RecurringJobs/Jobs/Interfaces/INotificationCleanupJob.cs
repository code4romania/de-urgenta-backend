using System.Threading;
using System.Threading.Tasks;

namespace DeUrgenta.RecurringJobs.Jobs.Interfaces
{
    public interface INotificationCleanupJob
    {
        Task RunAsync(CancellationToken cancellationToken);
    }
}
