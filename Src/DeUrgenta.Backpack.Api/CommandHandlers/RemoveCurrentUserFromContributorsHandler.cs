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
    public class RemoveCurrentUserFromContributorsHandler : IRequestHandler<RemoveCurrentUserFromContributors, Result<Unit, ValidationResult>>
    {
        private readonly IValidateRequest<RemoveCurrentUserFromContributors> _validator;
        private readonly DeUrgentaContext _context;

        public RemoveCurrentUserFromContributorsHandler(IValidateRequest<RemoveCurrentUserFromContributors> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<Unit, ValidationResult>> Handle(RemoveCurrentUserFromContributors request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);
            var backpackToUser = await _context
                .BackpacksToUsers
                .FirstAsync(btu => btu.User.Id == user.Id && btu.Backpack.Id == request.BackpackId, cancellationToken);
            _context.BackpacksToUsers.Remove(backpackToUser);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}