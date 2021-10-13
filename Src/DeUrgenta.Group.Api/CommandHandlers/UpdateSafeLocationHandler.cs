using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.CommandHandlers
{
    public class UpdateSafeLocationHandler : IRequestHandler<UpdateSafeLocation, Result<SafeLocationResponseModel, ValidationResult>>
    {
        private readonly IValidateRequest<UpdateSafeLocation> _validator;
        private readonly DeUrgentaContext _context;

        public UpdateSafeLocationHandler(IValidateRequest<UpdateSafeLocation> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }
        public async Task<Result<SafeLocationResponseModel, ValidationResult>> Handle(UpdateSafeLocation request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var safeLocation = await _context
                .GroupsSafeLocations
                .Include(gsl=>gsl.Group)
                .FirstAsync(gsl => gsl.Id == request.SafeLocationId, cancellationToken);

            safeLocation.Latitude = request.SafeLocation.Latitude;
            safeLocation.Longitude = request.SafeLocation.Longitude;
            safeLocation.Name = request.SafeLocation.Name;
            await _context.SaveChangesAsync(cancellationToken);


            return new SafeLocationResponseModel
            {
                Latitude = safeLocation.Latitude,
                Longitude = safeLocation.Longitude,
                Name = safeLocation.Name,
                Id = safeLocation.Id,
                GroupId = safeLocation.Group.Id
            };
        }
    }
}