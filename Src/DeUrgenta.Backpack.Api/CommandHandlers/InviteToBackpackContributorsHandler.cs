using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.CommandHandlers
{
    public class InviteToBackpackContributorsHandler : IRequestHandler<InviteToBackpackContributors, Result>
    {
        private readonly IValidateRequest<InviteToBackpackContributors> _validator;
        private readonly DeUrgentaContext _context;

        public InviteToBackpackContributorsHandler(IValidateRequest<InviteToBackpackContributors> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result> Handle(InviteToBackpackContributors request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure("Validation failed");
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);
            var backpack = await _context.Backpacks.FirstAsync(b => b.Id == request.BackpackId, cancellationToken);

            var invitedUser = await _context.Users.FirstAsync(u => u.Id == request.UserId, cancellationToken);

            await _context.BackpackInvites.AddAsync(new BackpackInvite
            {
                Backpack = backpack,
                InvitationSender = user,
                InvitationReceiver = invitedUser
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}