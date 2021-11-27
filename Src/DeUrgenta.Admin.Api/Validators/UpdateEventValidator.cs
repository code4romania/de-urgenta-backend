using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.I18n.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.Validators
{
    public class UpdateEventValidator : IValidateRequest<UpdateEvent>
    {
        private readonly DeUrgentaContext _context;

        public UpdateEventValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(UpdateEvent request)
        {
            var eventExists = await _context.Events.AnyAsync(x => x.Id == request.EventId);
            if (!eventExists)
            {
                return new LocalizableValidationError("event-not-exist",new LocalizableString("event-not-exist-message", request.EventId));
            }

            var eventTypeExists = await _context.EventTypes.AnyAsync(x => x.Id == request.Event.EventTypeId);

            return eventTypeExists ? ValidationResult.Ok : new LocalizableValidationError("event-type-not-exist",new LocalizableString("event-type-not-exist-message", request.Event.EventTypeId));
        }
    }
}
