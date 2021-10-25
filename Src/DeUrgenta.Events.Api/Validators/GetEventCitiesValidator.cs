using System.Threading.Tasks;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using Microsoft.EntityFrameworkCore;
using DeUrgenta.I18n.Service.Providers;

namespace DeUrgenta.Events.Api.Validators
{
    public class GetEventCitiesValidator : IValidateRequest<GetEventCities>
    {
        private readonly DeUrgentaContext _context;
        private readonly IamI18nProvider _i18nProvider;

        public GetEventCitiesValidator(DeUrgentaContext context, IamI18nProvider i18nProvider)
        {
            _context = context;
            _i18nProvider = i18nProvider;
        }

        public async Task<ValidationResult> IsValidAsync(GetEventCities request)
        {
            if (!await _context.EventTypes.AnyAsync(x => x.Id == request.EventTypeId))
            {
                return new DetailedValidationError("Event type does not exist", $"Requested event type id {request.EventTypeId} does not exist");
            }

            return ValidationResult.Ok;
        }
    }
}
