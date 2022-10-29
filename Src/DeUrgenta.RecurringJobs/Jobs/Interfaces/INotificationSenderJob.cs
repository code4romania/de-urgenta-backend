using System.Threading;
using System.Threading.Tasks;

namespace DeUrgenta.RecurringJobs.Jobs.Interfaces
{
    public interface INotificationSenderJob
    {
        Task RunAsync(CancellationToken cancellationToken);
    }
}
