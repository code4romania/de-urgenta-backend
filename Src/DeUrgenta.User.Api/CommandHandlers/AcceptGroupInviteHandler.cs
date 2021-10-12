using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.User.Api.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.User.Api.CommandHandlers
{
    public class AcceptGroupInviteHandler : IRequestHandler<AcceptGroupInvite, Result<Unit, ValidationResult>>
    {
        private readonly IValidateRequest<AcceptGroupInvite> _validator;
        private readonly DeUrgentaContext _context;

        public AcceptGroupInviteHandler(IValidateRequest<AcceptGroupInvite> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<Unit, ValidationResult>> Handle(AcceptGroupInvite request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            // remove invite
            var invite = await _context
                .GroupInvites
                .FirstAsync(gi => gi.Id == request.GroupInviteId, cancellationToken);

            _context.GroupInvites.Remove(invite);

            // add user to group members
            var group = await _context
                .Groups
                .Include(g => g.Backpack)
                .FirstAsync(g => g.Id == invite.GroupId, cancellationToken);

            var user = await _context.Users.FirstAsync(u => u.Id == invite.InvitationReceiverId, cancellationToken);

            await _context.UsersToGroups.AddAsync(new UserToGroup
            {
                User = user,
                Group = group
            },
                cancellationToken);

            await _context.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = group.Backpack, User = user }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}