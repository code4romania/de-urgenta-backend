using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.QueryHandlers
{
    public class GetBackpackContributorsHandler : IRequestHandler<GetBackpackContributors,Result<IImmutableList<BackpackContributorModel>, ValidationResult>>
    {
        private readonly IValidateRequest<GetBackpackContributors> _validator;
        private readonly DeUrgentaContext _context;

        public GetBackpackContributorsHandler(IValidateRequest<GetBackpackContributors> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<BackpackContributorModel>, ValidationResult>> Handle(GetBackpackContributors request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var contributors = await _context.BackpacksToUsers
                .Where(btu => btu.Backpack.Id == request.BackpackId)
                .Select(btu => new BackpackContributorModel
                {
                    FirstName = btu.User.FirstName, LastName = btu.User.LastName, UserId = btu.User.Id,
                })
                .ToListAsync(cancellationToken);

            return contributors.ToImmutableList();
        }
    }
}