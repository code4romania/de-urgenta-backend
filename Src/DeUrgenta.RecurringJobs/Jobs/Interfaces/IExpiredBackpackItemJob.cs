using System.Threading;
using System.Threading.Tasks;

namespace DeUrgenta.RecurringJobs.Jobs.Interfaces
{
    public interface IExpiredBackpackItemJob
    {
        Task RunAsync(CancellationToken cancellationToken);
    }
}
