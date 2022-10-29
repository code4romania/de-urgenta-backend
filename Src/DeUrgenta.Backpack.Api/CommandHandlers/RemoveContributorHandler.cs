using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.CommandHandlers
{
    public class RemoveContributorHandler : IRequestHandler<RemoveContributor, Result<Unit, ValidationResult>>
    {
        private readonly IValidateRequest<RemoveContributor> _validator;
        private readonly DeUrgentaContext _context;

        public RemoveContributorHandler(IValidateRequest<RemoveContributor> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<Unit, ValidationResult>> Handle(RemoveContributor request, CancellationToken ct)
        {
            var validationResult = await _validator.IsValidAsync(request, ct);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var backpackToUser = await _context
                .BackpacksToUsers
                .FirstAsync(btu => btu.User.Id == request.UserId && btu.Backpack.Id == request.BackpackId, ct);
            _context.BackpacksToUsers.Remove(backpackToUser);

            await _context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}