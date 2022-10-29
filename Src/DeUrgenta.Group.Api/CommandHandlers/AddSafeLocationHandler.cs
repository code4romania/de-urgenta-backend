using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.CommandHandlers
{
    public class AddSafeLocationHandler : IRequestHandler<AddSafeLocation, Result<SafeLocationResponseModel, ValidationResult>>
    {
        private readonly IValidateRequest<AddSafeLocation> _validator;
        private readonly DeUrgentaContext _context;

        public AddSafeLocationHandler(IValidateRequest<AddSafeLocation> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<SafeLocationResponseModel, ValidationResult>> Handle(AddSafeLocation request, CancellationToken ct)
        {
            var validationResult = await _validator.IsValidAsync(request, ct);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var group = await _context
                .Groups
                .FirstOrDefaultAsync(g => g.Id == request.GroupId, ct);

            var groupSafeLocation = new GroupSafeLocation
            {
                Latitude = request.SafeLocation.Latitude,
                Longitude = request.SafeLocation.Longitude,
                Name = request.SafeLocation.Name,
                Group = group
            };

            var newLocation = await _context.GroupsSafeLocations.AddAsync(groupSafeLocation, ct);
            await _context.SaveChangesAsync(ct);

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