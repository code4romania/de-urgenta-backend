using System.Threading.Tasks;

namespace DeUrgenta.RecurringJobs.Jobs
{
    public interface IEventArchivalJob
    {
        Task RunAsync();
    }
}