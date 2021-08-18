using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.CommandHandlers
{
    public class RemoveCurrentUserFromContributorsHandler : IRequestHandler<RemoveCurrentUserFromContributors, Result>
    {
        private readonly IValidateRequest<RemoveCurrentUserFromContributors> _validator;
        private readonly DeUrgentaContext _context;

        public RemoveCurrentUserFromContributorsHandler(IValidateRequest<RemoveCurrentUserFromContributors> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result> Handle(RemoveCurrentUserFromContributors request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure("Validation failed");
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);
            var backpackToUser = await _context
                .BackpacksToUsers
                .FirstAsync(btu => btu.User.Id == user.Id && btu.Backpack.Id == request.BackpackId, cancellationToken);
            _context.BackpacksToUsers.Remove(backpackToUser);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}