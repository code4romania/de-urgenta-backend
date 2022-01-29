using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.I18n.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Events.Api.Validators
{
    public class GetEventValidator : IValidateRequest<GetEvent>
    {
        private readonly DeUrgentaContext _context;

        public GetEventValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(GetEvent request)
        {
            var eventTypeExists = await _context.EventTypes.AnyAsync(x => x.Id == request.Filter.EventTypeId);

            return eventTypeExists ? ValidationResult.Ok : new LocalizableValidationError("event-type-not-exist",new LocalizableString("event-type-not-exist-message", request.Filter.EventTypeId));
        }
    }
}
