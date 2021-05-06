using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.QueryHandlers
{
    public class GetBackpacksHandler : IRequestHandler<GetBackpacks, Result<IImmutableList<BackpackModel>>>
    {
        private readonly IValidateRequest<GetBackpacks> _validator;
        private readonly DeUrgentaContext _context;

        public GetBackpacksHandler(IValidateRequest<GetBackpacks> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<BackpackModel>>> Handle(GetBackpacks request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<IImmutableList<BackpackModel>>("Validation failed");
            }

            var backpacks = await _context.BackpacksToUsers
                .Where(btu => btu.User.Sub == request.UserSub)
                .Select(btu => new BackpackModel
                {
                    Name = btu.Backpack.Name,
                    Id = btu.Backpack.Id,
                })
                .ToListAsync(cancellationToken);

            return backpacks.ToImmutableList();
        }
    }
}