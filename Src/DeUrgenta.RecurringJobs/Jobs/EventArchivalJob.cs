using System;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.RecurringJobs.Jobs.Interfaces;
using Microsoft.EntityFrameworkCore;
using DeUrgenta.Domain.Api;

namespace DeUrgenta.RecurringJobs.Jobs
{
    public class EventArchivalJob : IEventArchivalJob
    {
        private readonly DeUrgentaContext _context;

        public EventArchivalJob(DeUrgentaContext context)
        {
            _context = context;
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