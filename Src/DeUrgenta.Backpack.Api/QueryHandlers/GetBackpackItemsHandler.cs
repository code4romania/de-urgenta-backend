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
    public class GetBackpackItemsHandler : IRequestHandler<GetBackpackItems, Result<IImmutableList<BackpackItemModel>, ValidationResult>>
    {
        private readonly IValidateRequest<GetBackpackItems> _validator;
        private readonly DeUrgentaContext _context;

        public GetBackpackItemsHandler(IValidateRequest<GetBackpackItems> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }
        public async Task<Result<IImmutableList<BackpackItemModel>, ValidationResult>> Handle(GetBackpackItems request, CancellationToken ct)
        {
            var validationResult = await _validator.IsValidAsync(request, ct);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var backpackItems = await _context.BackpackItems
                .Where(item => item.Backpack.Id == request.BackpackId)
                .Select(item => new BackpackItemModel
                {
                    Id = item.Id,
                    BackpackId = item.BackpackId,
                    Amount = item.Amount,
                    Name = item.Name,
                    Category = item.Category,
                    ExpirationDate = item.ExpirationDate,
                    Version = item.Version
                })
                .ToListAsync(ct);

            return backpackItems.ToImmutableList();
        }
    }
}
