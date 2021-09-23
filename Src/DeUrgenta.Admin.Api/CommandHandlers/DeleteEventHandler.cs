using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.CommandHandlers
{
    public class DeleteEventHandler : IRequestHandler<DeleteEvent, Result>
    {
        private readonly IValidateRequest<DeleteEvent> _validator;
        private readonly DeUrgentaContext _context;

        public DeleteEventHandler(IValidateRequest<DeleteEvent> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result> Handle(DeleteEvent request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure("Validation failed");
            }

            var eventToBeDeleted = await _context.Events.FirstAsync(b => b.Id == request.EventId, cancellationToken);
            _context.Events.Remove(eventToBeDeleted);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
