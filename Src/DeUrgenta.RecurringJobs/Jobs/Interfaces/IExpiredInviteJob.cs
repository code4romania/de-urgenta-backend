using System.Threading;
using System.Threading.Tasks;

namespace DeUrgenta.RecurringJobs.Jobs.Interfaces
{
    public interface IExpiredInviteJob
    {
        Task RunAsync(CancellationToken cancellationToken);
    }
}
