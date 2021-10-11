using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Models;
using Microsoft.EntityFrameworkCore;
using InviteType = DeUrgenta.Invite.Api.Models.InviteType;

namespace DeUrgenta.Invite.Api.CommandHandlers
{
    public class AcceptGroupInviteHandler : IInviteHandler
    {
        private readonly DeUrgentaContext _context;

        public AcceptGroupInviteHandler(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<Result<AcceptInviteModel>> HandleAsync(AcceptInvite request, CancellationToken cancellationToken)
        {
            var group = await _context.Groups.FirstAsync(g => g.Id == request.DestinationId, cancellationToken);
            var user = await _context.Users.FirstAsync(u => u.Id == request.UserId, cancellationToken);

            await _context.UsersToGroups.AddAsync(new UserToGroup { User = user, Group = group }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new AcceptInviteModel { DestinationId = group.Id, Type = InviteType.Backpack };
        }
    }
}
