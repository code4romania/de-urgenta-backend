using System.Threading.Tasks;

namespace DeUrgenta.RecurringJobs.Jobs
{
    public interface INotificationSenderJob
    {
        Task RunAsync();
    }
}
