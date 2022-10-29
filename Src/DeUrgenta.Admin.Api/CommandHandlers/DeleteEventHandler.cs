using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.CommandHandlers
{
    public class DeleteEventHandler : IRequestHandler<DeleteEvent, Result<Unit, ValidationResult>>
    {
        private readonly IValidateRequest<DeleteEvent> _validator;
        private readonly DeUrgentaContext _context;

        public DeleteEventHandler(IValidateRequest<DeleteEvent> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<Unit, ValidationResult>> Handle(DeleteEvent request, CancellationToken ct)
        {
            var validationResult = await _validator.IsValidAsync(request, ct);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var eventToBeDeleted = await _context.Events.FirstAsync(b => b.Id == request.EventId, ct);
            _context.Events.Remove(eventToBeDeleted);
            await _context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
