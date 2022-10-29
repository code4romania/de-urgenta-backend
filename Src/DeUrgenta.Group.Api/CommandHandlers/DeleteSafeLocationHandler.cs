using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Group.Api.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.CommandHandlers
{
    public class DeleteSafeLocationHandler : IRequestHandler<DeleteSafeLocation, Result<Unit, ValidationResult>>
    {
        private readonly IValidateRequest<DeleteSafeLocation> _validator;
        private readonly DeUrgentaContext _context;

        public DeleteSafeLocationHandler(IValidateRequest<DeleteSafeLocation> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<Unit, ValidationResult>> Handle(DeleteSafeLocation request, CancellationToken ct)
        {
            var validationResult = await _validator.IsValidAsync(request, ct);

            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var safeLocation =await _context
                .GroupsSafeLocations
                .FirstAsync(gsl => gsl.Id == request.SafeLocationId, ct);

            _context.GroupsSafeLocations.Remove(safeLocation);
            await _context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}