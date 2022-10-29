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
    public class AcceptGroupInviteHandler : IRequestHandler<AcceptGroupInvite, Result<AcceptInviteModel, ValidationResult>>
    {
        private readonly DeUrgentaContext _context;
        private readonly IValidateRequest<AcceptGroupInvite> _validator;

        public AcceptGroupInviteHandler(DeUrgentaContext context, IValidateRequest<AcceptGroupInvite> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<AcceptInviteModel, ValidationResult>> Handle(AcceptGroupInvite request, CancellationToken ct)
        {
            var validationResult = await _validator.IsValidAsync(request, ct);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }
            
            var group = await _context.Groups.FirstAsync(g => g.Id == request.GroupId, ct);
            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, ct);

            await _context.UsersToGroups.AddAsync(new UserToGroup { User = user, Group = group }, ct);
            await _context.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = group.Backpack, User = user }, ct);

            await _context.SaveChangesAsync(ct);

            return new AcceptInviteModel
            {
                DestinationId = group.Id, 
                DestinationName = group.Name,
                Type = InviteType.Group
            };
        }
    }
}
