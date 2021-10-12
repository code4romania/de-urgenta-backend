using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.CommandHandlers
{
    public class UpdateBackpackItemHandler : IRequestHandler<UpdateBackpackItem, Result<BackpackItemModel>>
    {
        private readonly IValidateRequest<UpdateBackpackItem> _validator;
        private readonly DeUrgentaContext _context;

        public UpdateBackpackItemHandler(IValidateRequest<UpdateBackpackItem> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<BackpackItemModel>> Handle(UpdateBackpackItem request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return Result.Failure<BackpackItemModel>("Validation failed");
            }
            var backpackItem = await _context.BackpackItems.FirstAsync(x => x.Id == request.ItemId, cancellationToken);
            backpackItem.Name = request.BackpackItem.Name;
            backpackItem.BackpackCategory = request.BackpackItem.CategoryType;
            backpackItem.Amount = request.BackpackItem.Amount;
            backpackItem.ExpirationDate = request.BackpackItem.ExpirationDate;

            await _context.SaveChangesAsync(cancellationToken);

            return new BackpackItemModel
            {
                Id = backpackItem.Id,
                Amount = backpackItem.Amount,
                CategoryType = backpackItem.BackpackCategory,
                ExpirationDate = backpackItem.ExpirationDate
            };
        }
    }
}
