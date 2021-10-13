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
    public class DeleteBackpackHandler : IRequestHandler<DeleteBackpack, Result<Unit, ValidationResult>>
    {
        private readonly IValidateRequest<DeleteBackpack> _validator;
        private readonly DeUrgentaContext _context;

        public DeleteBackpackHandler(IValidateRequest<DeleteBackpack> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<Unit, ValidationResult>> Handle(DeleteBackpack request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var backpack = await _context.Backpacks.FirstAsync(b => b.Id == request.BackpackId, cancellationToken);
            _context.Backpacks.Remove(backpack);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}