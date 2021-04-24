using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Queries;
using DeUrgenta.Group.Api.Validators;
using MediatR;

namespace DeUrgenta.Group.Api.QueryHandlers
{
    public class GetGroupSafeLocationsHandler : IRequestHandler<GetGroupSafeLocations, Result<IImmutableList<SafeLocationModel>>>
    {
        private readonly IValidateRequest<GetGroupSafeLocations> _validator;
        private readonly DeUrgentaContext _dbContext;

        public GetGroupSafeLocationsHandler(IValidateRequest<GetGroupSafeLocations> validator, DeUrgentaContext dbContext)
        {
            _validator = validator;
            _dbContext = dbContext;
        }

        public async Task<Result<IImmutableList<SafeLocationModel>>> Handle(GetGroupSafeLocations request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<IImmutableList<SafeLocationModel>>("Validation failed");
            }

            return ImmutableList<SafeLocationModel>.Empty;
        }
    }
}