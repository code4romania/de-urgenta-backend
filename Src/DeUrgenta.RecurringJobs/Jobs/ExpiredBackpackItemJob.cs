using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.RecurringJobs.Domain;
using DeUrgenta.RecurringJobs.Jobs.Interfaces;

namespace DeUrgenta.RecurringJobs.Jobs
{
    public class ExpiredBackpackItemJob : IExpiredBackpackItemJob
    {
        private DeUrgentaContext _context;
        private JobsContext _jobsContext;

        public ExpiredBackpackItemJob(DeUrgentaContext context, JobsContext jobsContext)
        {
            _context = context;
            _jobsContext = jobsContext;
        }

        public Task RunAsync()
        {
            //get expired items 

            //add notification
        }
    }
}
