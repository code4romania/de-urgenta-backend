using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.CommandHandlers
{
    public class DeleteBackpackItemHandler : IRequestHandler<DeleteBackpackItem, Result<Unit, ValidationResult>>
    {
        private readonly IValidateRequest<DeleteBackpackItem> _validator;
        private readonly DeUrgentaContext _context;

        public DeleteBackpackItemHandler(IValidateRequest<DeleteBackpackItem> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<Unit, ValidationResult>> Handle(DeleteBackpackItem request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var backpackItem = await _context.BackpackItems.FirstAsync(x => x.Id == request.ItemId, cancellationToken);
            _context.BackpackItems.Remove(backpackItem);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
