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
    public class AcceptGroupInviteHandler : IRequestHandler<AcceptGroupInvite, Result<AcceptInviteModel>>
    {
        private readonly DeUrgentaContext _context;
        private readonly IValidateRequest<AcceptGroupInvite> _validator;

        public AcceptGroupInviteHandler(DeUrgentaContext context, IValidateRequest<AcceptGroupInvite> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<AcceptInviteModel>> Handle(AcceptGroupInvite request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<AcceptInviteModel>("Validation failed");
            }
            
            var group = await _context.Groups.FirstAsync(g => g.Id == request.GroupId, cancellationToken);
            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);

            await _context.UsersToGroups.AddAsync(new UserToGroup { User = user, Group = group }, cancellationToken);
            await _context.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = group.Backpack, User = user }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new AcceptInviteModel
            {
                DestinationId = group.Id, 
                DestinationName = group.Name,
                Type = InviteType.Group
            };
        }
    }
}
