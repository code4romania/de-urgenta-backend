using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.User.Api.Commands;
using DeUrgenta.User.Api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.User.Api.CommandHandlers
{
    public class AddLocationHandler : IRequestHandler<AddLocation, Result<UserLocationModel, ValidationResult>>
    {
        private readonly IValidateRequest<AddLocation> _validator;
        private readonly DeUrgentaContext _context;

        public AddLocationHandler(IValidateRequest<AddLocation> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<UserLocationModel, ValidationResult>> Handle(AddLocation request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);
            var location = new UserLocation
            {
                Latitude = request.Location.Latitude,
                Longitude = request.Location.Longitude,
                Address = request.Location.Address,
                Type = request.Location.Type,
                User = user
            };

            await _context.UserLocations.AddAsync(location, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new UserLocationModel
            {
                Id = location.Id,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Address = location.Address,
                Type = location.Type
            };
        }
    }
}