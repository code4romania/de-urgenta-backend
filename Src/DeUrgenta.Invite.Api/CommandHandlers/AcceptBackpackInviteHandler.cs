using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using InviteType = DeUrgenta.Invite.Api.Models.InviteType;

namespace DeUrgenta.Invite.Api.CommandHandlers
{
    public class AcceptBackpackInviteHandler : IRequestHandler<AcceptBackpackInvite, Result<AcceptInviteModel>>
    {
        private readonly DeUrgentaContext _context;
        private readonly IValidateRequest<AcceptBackpackInvite> _validator;

        public AcceptBackpackInviteHandler(DeUrgentaContext context, IValidateRequest<AcceptBackpackInvite> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<AcceptInviteModel>> Handle(AcceptBackpackInvite request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<AcceptInviteModel>("Validation failed");
            }

            var backpack = await _context.Backpacks.FirstAsync(b => b.Id == request.BackpackId, cancellationToken);
            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);

            await _context.BackpacksToUsers.AddAsync(new BackpackToUser { User = user, Backpack = backpack }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new AcceptInviteModel
            {
                DestinationId = backpack.Id,
                DestinationName = backpack.Name,
                Type = InviteType.Group
            };
        }

    }
}