using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.I18n.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.Validators
{
    public class DeleteEventValidator : IValidateRequest<DeleteEvent>
    {
        private readonly DeUrgentaContext _context;

        public DeleteEventValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(DeleteEvent request)
        {
            var @event = await _context.Events.FirstOrDefaultAsync(x => x.Id == request.EventId);

            if (@event == null)
            {
                return new LocalizableValidationError("event-not-exist", new LocalizableString("event-not-exist-message", request.EventId));
            }

            return ValidationResult.Ok;
        }
    }
}
