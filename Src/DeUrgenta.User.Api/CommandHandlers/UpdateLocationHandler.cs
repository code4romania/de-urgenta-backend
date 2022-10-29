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
    public class UpdateLocationHandler : IRequestHandler<UpdateLocation, Result<Unit, ValidationResult>>
    {
        private readonly IValidateRequest<UpdateLocation> _validator;
        private readonly DeUrgentaContext _context;

        public UpdateLocationHandler(IValidateRequest<UpdateLocation> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<Unit, ValidationResult>> Handle(UpdateLocation request, CancellationToken ct)
        {
            var validationResult = await _validator.IsValidAsync(request, ct);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var location = await _context.UserLocations.FirstAsync(l => l.Id == request.LocationId, ct);
            location.Latitude = request.Location.Latitude;
            location.Longitude = request.Location.Longitude;
            location.Address = request.Location.Address;
            location.Type = request.Location.Type;

            await _context.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}