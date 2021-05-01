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
    public class LeaveGroupHandler : IRequestHandler<LeaveGroup, Result>
    {
        private readonly IValidateRequest<LeaveGroup> _validator;
        private readonly DeUrgentaContext _context;

        public LeaveGroupHandler(IValidateRequest<LeaveGroup> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result> Handle(LeaveGroup request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure("Validation failed");
            }

            // TODO: remove user items in backpack
            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);
            var groupAssignment = await _context
                .UsersToGroups
                .FirstAsync(utg => utg.Group.Id == request.GroupId && utg.User.Id == user.Id, cancellationToken);

            _context.UsersToGroups.Remove(groupAssignment);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}