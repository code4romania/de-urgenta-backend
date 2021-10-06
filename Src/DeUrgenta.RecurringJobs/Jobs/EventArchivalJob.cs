using System;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.RecurringJobs.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.RecurringJobs.Jobs
{
    public class EventArchivalJob : IEventArchivalJob
    {
        private readonly DeUrgentaContext _context;
        private readonly JobsContext _jobsContext;
        
        public EventArchivalJob(DeUrgentaContext context, JobsContext jobsContext)
        {
            _context = context;
            _jobsContext = jobsContext;
        }
        
        public async Task RunAsync()
        {
            var expiredEvents = await _context.Events.Where(e => e.OccursOn.Date < DateTime.Today && !e.IsArchived).ToListAsync();

            foreach (var expiringEvent in expiredEvents)
            {
                expiringEvent.IsArchived = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}