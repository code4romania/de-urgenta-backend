using System.Threading.Tasks;

namespace DeUrgenta.RecurringJobs.Jobs
{
    public interface INotificationCleanupJob
    {
        Task RunAsync();
    }
}
