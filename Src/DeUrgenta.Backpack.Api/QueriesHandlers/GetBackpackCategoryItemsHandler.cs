using System;
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

namespace DeUrgenta.Backpack.Api.QueriesHandlers
{
    public class GetBackpackCategoryItemsHandler : IRequestHandler<GetBackpackCategoryItems, Result<IImmutableList<BackpackItemModel>>>
    {
        private readonly IValidateRequest<GetBackpackCategoryItems> _validator;
        private readonly DeUrgentaContext _context;

        public GetBackpackCategoryItemsHandler(IValidateRequest<GetBackpackCategoryItems> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }
        public async Task<Result<IImmutableList<BackpackItemModel>>> Handle(GetBackpackCategoryItems request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<IImmutableList<BackpackItemModel>>("Validation failed");
            }

            var backpackItems = await _context.BackpackItems
                .Where(item => item.BackpackId == request.BackpackId && item.BackpackCategory == request.CategoryId)
                .Select(item => new BackpackItemModel
                {
                    Id = item.Id,
                    Amount = item.Amount,
                    CategoryType = item.BackpackCategory,
                    ExpirationDate = item.ExpirationDate
                })
                .ToListAsync(cancellationToken);

            return backpackItems.ToImmutableList();
        }
    }
}
