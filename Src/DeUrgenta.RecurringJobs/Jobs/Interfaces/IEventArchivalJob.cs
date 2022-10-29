using System.Threading;
using System.Threading.Tasks;

namespace DeUrgenta.RecurringJobs.Jobs.Interfaces
{
    public interface IEventArchivalJob
    {
        Task RunAsync(CancellationToken cancellationToken);
    }
}