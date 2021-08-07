using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.User.Api.Commands;
using DeUrgenta.User.Api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.User.Api.CommandHandlers
{
    public class UpdateLocationHandler : IRequestHandler<UpdateLocation, Result>
    {
        private readonly IValidateRequest<UpdateLocation> _validator;
        private readonly DeUrgentaContext _context;

        public UpdateLocationHandler(IValidateRequest<UpdateLocation> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result> Handle(UpdateLocation request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<UserLocationModel>("Validation failed");
            }

            var location = await _context.UserLocations.FirstAsync(l => l.Id == request.LocationId, cancellationToken);
            location.Latitude = request.Location.Latitude;
            location.Longitude = request.Location.Longitude;
            location.Address = request.Location.Address;
            location.Type = request.Location.Type;

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}