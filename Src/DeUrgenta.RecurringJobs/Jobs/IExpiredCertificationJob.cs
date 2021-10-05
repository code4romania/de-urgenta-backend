using System.Threading.Tasks;

namespace DeUrgenta.RecurringJobs.Jobs
{
    public interface IExpiredCertificationJob
    {
        Task RunAsync();
    }
}