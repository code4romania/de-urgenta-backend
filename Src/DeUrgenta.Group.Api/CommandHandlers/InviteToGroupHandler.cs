using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Group.Api.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.CommandHandlers
{
    public class InviteToGroupHandler : IRequestHandler<InviteToGroup, Result<Unit, ValidationResult>>
    {
        private readonly IValidateRequest<InviteToGroup> _validator;
        private readonly DeUrgentaContext _context;

        public InviteToGroupHandler(IValidateRequest<InviteToGroup> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<Unit, ValidationResult>> Handle(InviteToGroup request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);
            var group = await _context.Groups.FirstAsync(g => g.Id == request.GroupId, cancellationToken);

            var invitedUser = await _context.Users.FirstAsync(u => u.Id == request.UserId, cancellationToken);

            _context.GroupInvites.Add(new GroupInvite
            {
                Group = group,
                InvitationReceiver = invitedUser,
                InvitationSender = user
            });

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}