using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.User.Api.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.User.Api.CommandHandlers
{
    public class DeleteLocationHandler : IRequestHandler<DeleteLocation, Result<Unit, ValidationResult>>
    {
        private readonly IValidateRequest<DeleteLocation> _validator;
        private readonly DeUrgentaContext _context;

        public DeleteLocationHandler(IValidateRequest<DeleteLocation> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<Unit, ValidationResult>> Handle(DeleteLocation request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var location = await _context.UserLocations.FirstAsync(l => request.LocationId == l.Id, cancellationToken);

            _context.UserLocations.Remove(location);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}