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
    public class AcceptBackpackInviteHandler : IRequestHandler<AcceptBackpackInvite, Result>
    {
        private readonly IValidateRequest<AcceptBackpackInvite> _validator;
        private readonly DeUrgentaContext _context;

        public AcceptBackpackInviteHandler(IValidateRequest<AcceptBackpackInvite> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result> Handle(AcceptBackpackInvite request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure("Validation failed");
            }

            // remove invite
            var invite = await _context
                .BackpackInvites
                .FirstAsync(bi => bi.Id == request.BackpackInviteId, cancellationToken);

            _context.BackpackInvites.Remove(invite);

            // add user to contributors
            var backpack = await _context.Backpacks.FirstAsync(b => b.Id == invite.BackpackId, cancellationToken);
            var user = await _context.Users.FirstAsync(u => u.Id == invite.InvitationReceiverId, cancellationToken);

            await _context.BackpacksToUsers.AddAsync(new BackpackToUser
            {
                User = user,
                Backpack = backpack
            },
                cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}