using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.RecurringJobs.Jobs.Config;
using DeUrgenta.RecurringJobs.Jobs.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.RecurringJobs.Jobs
{
    public class AcceptedInviteJob : IAcceptedInviteJob
    {
        private readonly DeUrgentaContext _context;
        private AcceptedInviteJobConfig _config;

        public AcceptedInviteJob(DeUrgentaContext context, IOptions<AcceptedInviteJobConfig> config)
        {
            _context = context;
            _config = config.Value;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var oldAcceptedInvites = await _context.Invites.Where(invite =>
                    (DateTime.Today - invite.SentOn).Days >= _config.DaysBeforeExpirationDate
                    && invite.InviteStatus == InviteStatus.Accepted)
                .ToListAsync(cancellationToken);

            _context.RemoveRange(oldAcceptedInvites);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
