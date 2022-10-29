using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using InviteType = DeUrgenta.Invite.Api.Models.InviteType;

namespace DeUrgenta.Invite.Api.CommandHandlers
{
    public class AcceptBackpackInviteHandler : IRequestHandler<AcceptBackpackInvite, Result<AcceptInviteModel, ValidationResult>>
    {
        private readonly DeUrgentaContext _context;
        private readonly IValidateRequest<AcceptBackpackInvite> _validator;

        public AcceptBackpackInviteHandler(DeUrgentaContext context, IValidateRequest<AcceptBackpackInvite> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<AcceptInviteModel, ValidationResult>> Handle(AcceptBackpackInvite request, CancellationToken ct)
        {
            var validationResult = await _validator.IsValidAsync(request, ct);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var backpack = await _context.Backpacks.FirstAsync(b => b.Id == request.BackpackId, ct);
            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, ct);

            await _context.BackpacksToUsers.AddAsync(new BackpackToUser { User = user, Backpack = backpack }, ct);
            await _context.SaveChangesAsync(ct);

            return new AcceptInviteModel
            {
                DestinationId = backpack.Id,
                DestinationName = backpack.Name,
                Type = InviteType.Group
            };
        }

    }
}