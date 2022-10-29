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

        public async Task<Result<BackpackItemModel, ValidationResult>> Handle(UpdateBackpackItem request, CancellationToken ct)
        {
            var validationResult = await _validator.IsValidAsync(request, ct);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }
            var backpackItem = await _context.BackpackItems.FirstAsync(x => x.Id == request.ItemId, ct);
            backpackItem.Name = request.BackpackItem.Name;
            backpackItem.Category = request.BackpackItem.Category;
            backpackItem.Amount = request.BackpackItem.Amount;
            backpackItem.ExpirationDate = request.BackpackItem.ExpirationDate;
            backpackItem.Version += 1;

            await _context.SaveChangesAsync(ct);

            return new BackpackItemModel
            {
                Id = backpackItem.Id,
                BackpackId = backpackItem.BackpackId,
                Name = backpackItem.Name,
                Amount = backpackItem.Amount,
                Category = backpackItem.Category,
                ExpirationDate = backpackItem.ExpirationDate,
                Version = backpackItem.Version
            };
        }
    }
}
