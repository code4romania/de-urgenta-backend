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
    public class ExpiredInviteJob : IExpiredInviteJob
    {
        private readonly DeUrgentaContext _context;
        private readonly ExpiredInviteJobConfig _config;

        public ExpiredInviteJob(DeUrgentaContext context, IOptions<ExpiredInviteJobConfig> config)
        {
            _context = context;
            _config = config.Value;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var expiredInvites = await _context.Invites
               .Where(invite => (DateTime.Today - invite.SentOn).Days >= _config.DaysBeforeExpirationDate
                                && invite.InviteStatus == InviteStatus.Sent)
               .ToListAsync(cancellationToken: cancellationToken);

            _context.Invites.RemoveRange(expiredInvites);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
