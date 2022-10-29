using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Options;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.Backpack.Api.QueryHandlers
{
    public class GetMyBackpacksHandler : IRequestHandler<GetMyBackpacks, Result<IImmutableList<BackpackModel>, ValidationResult>>
    {
        private readonly IValidateRequest<GetMyBackpacks> _validator;
        private readonly DeUrgentaContext _context;
        private readonly BackpacksConfig _config;

        public GetMyBackpacksHandler(IValidateRequest<GetMyBackpacks> validator, DeUrgentaContext context, IOptions<BackpacksConfig> config)
        {
            _validator = validator;
            _context = context;
            _config = config.Value;
        }

        public async Task<Result<IImmutableList<BackpackModel>, ValidationResult>> Handle(GetMyBackpacks request, CancellationToken ct)
        {
            var validationResult = await _validator.IsValidAsync(request, ct);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var backpacks = await _context.BackpacksToUsers
                .Where(btu => btu.User.Sub == request.UserSub)
                .Where(btu => btu.IsOwner)
                .GroupBy(x => new { Id = x.BackpackId, x.Backpack.Name }, btu => btu.Backpack, (key, g) =>
                           new BackpackModel
                           {
                               Name = key.Name,
                               Id = key.Id,
                               MaxNumberOfContributors = _config.MaxContributors,
                               NumberOfContributors = g.Count()
                           })
                .ToListAsync(ct);

            return backpacks.ToImmutableList();
        }
    }
}