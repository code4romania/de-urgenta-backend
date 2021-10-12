using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.CommandHandlers
{
    public class AddSafeLocationHandler : IRequestHandler<AddSafeLocation, Result<SafeLocationResponseModel>>
    {
        private readonly IValidateRequest<AddSafeLocation> _validator;
        private readonly DeUrgentaContext _context;

        public AddSafeLocationHandler(IValidateRequest<AddSafeLocation> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<SafeLocationResponseModel>> Handle(AddSafeLocation request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return Result.Failure<SafeLocationResponseModel>("Validation failed");
            }

            var group = await _context
                .Groups
                .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);

            var groupSafeLocation = new GroupSafeLocation
            {
                Latitude = request.SafeLocation.Latitude,
                Longitude = request.SafeLocation.Longitude,
                Name = request.SafeLocation.Name,
                Group = group
            };

            var newLocation = await _context.GroupsSafeLocations.AddAsync(groupSafeLocation, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new SafeLocationResponseModel
            {
                Latitude = newLocation.Entity.Latitude,
                Longitude = newLocation.Entity.Longitude,
                Name = newLocation.Entity.Name,
                Id = newLocation.Entity.Id,
                GroupId = newLocation.Entity.Group.Id
            };
        }
    }
}