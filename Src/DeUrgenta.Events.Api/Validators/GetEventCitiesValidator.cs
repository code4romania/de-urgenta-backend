using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.I18n.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Events.Api.Validators
{
    public class GetEventCitiesValidator : IValidateRequest<GetEventCities>
    {
        private readonly DeUrgentaContext _context;

        public GetEventCitiesValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(GetEventCities request, CancellationToken ct)
        {
            if (!await _context.EventTypes.AnyAsync(x => x.Id == request.EventTypeId, ct))
            {
                return new LocalizableValidationError("event-type-not-exist",new LocalizableString("event-type-not-exist-message", request.EventTypeId));
            }

            return ValidationResult.Ok;
        }
    }
}
