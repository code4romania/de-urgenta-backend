using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.CommandHandlers
{
    public class UpdateBackpackItemHandler : IRequestHandler<UpdateBackpackItem, Result<BackpackItemModel, ValidationResult>>
    {
        private readonly IValidateRequest<UpdateBackpackItem> _validator;
        private readonly DeUrgentaContext _context;

        public UpdateBackpackItemHandler(IValidateRequest<UpdateBackpackItem> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<BackpackItemModel, ValidationResult>> Handle(UpdateBackpackItem request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }
            var backpackItem = await _context.BackpackItems.FirstAsync(x => x.Id == request.ItemId, cancellationToken);
            backpackItem.Name = request.BackpackItem.Name;
            backpackItem.BackpackCategory = request.BackpackItem.CategoryType;
            backpackItem.Amount = request.BackpackItem.Amount;
            backpackItem.ExpirationDate = request.BackpackItem.ExpirationDate;
            backpackItem.Version += 1;

            await _context.SaveChangesAsync(cancellationToken);

            return new BackpackItemModel
            {
                Id = backpackItem.Id,
                BackpackId = backpackItem.BackpackId,
                Name = backpackItem.Name,
                Amount = backpackItem.Amount,
                CategoryType = backpackItem.BackpackCategory,
                ExpirationDate = backpackItem.ExpirationDate,
                Version = backpackItem.Version
            };
        }
    }
}
