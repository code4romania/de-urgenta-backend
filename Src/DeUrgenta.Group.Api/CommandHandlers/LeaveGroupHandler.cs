using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.CommandHandlers
{
    public class LeaveGroupHandler : IRequestHandler<LeaveGroup, Result<Unit, ValidationResult>>
    {
        private readonly IValidateRequest<LeaveGroup> _validator;
        private readonly DeUrgentaContext _context;

        public LeaveGroupHandler(IValidateRequest<LeaveGroup> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<Unit, ValidationResult>> Handle(LeaveGroup request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            
            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);
            var groupAssignment = await _context
                .UsersToGroups
                .FirstAsync(utg => utg.Group.Id == request.GroupId && utg.User.Id == user.Id, cancellationToken);

            var backpackToUser = await _context
                .BackpacksToUsers
                .FirstAsync(btu => btu.Backpack.Id == groupAssignment.Group.Backpack.Id && btu.User.Id == user.Id, cancellationToken);

            _context.BackpacksToUsers.Remove(backpackToUser);
            _context.UsersToGroups.Remove(groupAssignment);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}