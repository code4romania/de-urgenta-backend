using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Validators;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.CommandHandlers
{
    public class RemoveFromGroupHandler : IRequestHandler<RemoveFromGroup, Result>
    {
        private readonly IValidateRequest<RemoveFromGroup> _validator;
        private readonly DeUrgentaContext _context;

        public RemoveFromGroupHandler(IValidateRequest<RemoveFromGroup> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result> Handle(RemoveFromGroup request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure("Validation failed");
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);
            var group = await _context.Groups.FirstAsync(u => u.AdminId == user.Id, cancellationToken);
            var userToRemove = await _context
                .UsersToGroups
                .FirstAsync(utg => utg.UserId == request.UserId && utg.GroupId == group.Id, cancellationToken);

            _context.UsersToGroups.Remove(userToRemove);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}