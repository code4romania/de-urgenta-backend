using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
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
    public class GetBackpackItemsHandler //: IRequestHandler<GetBackpackItems, Result<IImmutableList<BackpackItemModel>>>
    {
        private readonly IValidateRequest<GetBackpackItems> _validator;
        private readonly DeUrgentaContext _context;

        public GetBackpackItemsHandler(IValidateRequest<GetBackpackItems> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }
        public async Task<Result<IImmutableList<BackpackItemModel>>> Handle(GetBackpackItems request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<IImmutableList<BackpackItemModel>>("Validation failed");
            }

            var backpackItems = await _context.BackpackItem
                .Where(item => item.Backpack.Id == request.BackpackId)
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
