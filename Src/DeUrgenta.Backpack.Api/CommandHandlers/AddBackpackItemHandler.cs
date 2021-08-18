using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.CommandHandlers
{
    public class AddBackpackItemHandler : IRequestHandler<AddBackpackItem, Result<BackpackItemModel>>
    {
        private readonly IValidateRequest<AddBackpackItem> _validator;
        private readonly DeUrgentaContext _context;

        public AddBackpackItemHandler(IValidateRequest<AddBackpackItem> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<BackpackItemModel>> Handle(AddBackpackItem request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<BackpackItemModel>("Validation failed");
            }

            var backpack = await _context.Backpacks.FirstAsync(x => x.Id == request.BackpackId, cancellationToken);
            var backpackItem = new BackpackItem
            {
                Name = request.BackpackItem.Name,
                BackpackCategory = (BackpackCategoryType)request.BackpackItem.CategoryType,
                Amount = request.BackpackItem.Amount,
                ExpirationDate = request.BackpackItem.ExpirationDate,
                Backpack = backpack
            };

            await _context.AddAsync(backpackItem, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new BackpackItemModel
            {
                Id = backpackItem.Id,
                Name = backpackItem.Name,
                Amount = backpackItem.Amount,
                CategoryType = backpackItem.BackpackCategory,
                ExpirationDate = backpackItem.ExpirationDate,
                BackpackId = request.BackpackId
            };
        }
    }
}
