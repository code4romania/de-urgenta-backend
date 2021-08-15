using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.User.Api.Models;
using DeUrgenta.User.Api.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.User.Api.QueryHandlers
{
    public class GetUserLocationsHandler : IRequestHandler<GetUserLocations, Result<IImmutableList<UserLocationModel>>>
    {
        private readonly IValidateRequest<GetUserLocations> _validator;
        private readonly DeUrgentaContext _context;

        public GetUserLocationsHandler(IValidateRequest<GetUserLocations> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<UserLocationModel>>> Handle(GetUserLocations request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<IImmutableList<UserLocationModel>>("Validation failed");
            }

            var user = await _context.Users
                .Include(u => u.Locations)
                .Where(x => x.Sub == request.UserSub)
                .FirstAsync(cancellationToken);

            return user.Locations.Select(location => new UserLocationModel
            {
                Id = location.Id,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Address = location.Address,
                Type = location.Type
            }).ToImmutableList();
        }
    }
}