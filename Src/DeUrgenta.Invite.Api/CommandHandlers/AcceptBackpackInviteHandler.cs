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
    public class AcceptBackpackInviteHandler : IInviteHandler
    {
        private readonly DeUrgentaContext _context;

        public AcceptBackpackInviteHandler(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<Result<AcceptInviteModel>> HandleAsync(AcceptInvite request, CancellationToken cancellationToken)
        {
            var backpack = await _context.Backpacks.FirstAsync(b => b.Id == request.DestinationId, cancellationToken);
            var user = await _context.Users.FirstAsync(u => u.Id == request.UserId, cancellationToken);

            await _context.BackpacksToUsers.AddAsync(new BackpackToUser { User = user, Backpack = backpack }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new AcceptInviteModel { DestinationId = backpack.Id, Type = InviteType.Group };
        }
    }
}