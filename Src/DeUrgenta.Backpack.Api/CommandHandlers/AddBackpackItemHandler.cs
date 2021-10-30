using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.CommandHandlers
{
    public class AddBackpackItemHandler : IRequestHandler<AddBackpackItem, Result<BackpackItemModel, ValidationResult>>
    {
        private readonly IValidateRequest<AddBackpackItem> _validator;
        private readonly DeUrgentaContext _context;

        public AddBackpackItemHandler(IValidateRequest<AddBackpackItem> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<BackpackItemModel, ValidationResult>> Handle(AddBackpackItem request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var backpack = await _context.Backpacks.FirstAsync(x => x.Id == request.BackpackId, cancellationToken);
            var backpackItem = new BackpackItem
            {
                Name = request.BackpackItem.Name,
                BackpackCategory = request.BackpackItem.CategoryType,
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
                BackpackId = request.BackpackId,
                Version = 0
            };
        }
    }
}
