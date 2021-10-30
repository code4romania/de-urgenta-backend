using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.I18n.Service.Providers;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.Validators
{
    public class CreateEventValidator : IValidateRequest<CreateEvent>
    {
        private readonly DeUrgentaContext _context;
        private readonly IamI18nProvider _i18nProvider;

        public CreateEventValidator(DeUrgentaContext context, IamI18nProvider i18nProvider)
        {
            _context = context;
            _i18nProvider = i18nProvider;
        }

        public async Task<ValidationResult> IsValidAsync(CreateEvent request)
        {
            var eventTypeExists = await _context.EventTypes.AnyAsync(x => x.Id == request.Event.EventTypeId);

            return eventTypeExists
                ? ValidationResult.Ok
                : new DetailedValidationError(
                    await _i18nProvider.Localize("event-type-not-exist"),
                    await _i18nProvider.Localize("event-type-not-exist-message", request.Event.EventTypeId));
        }
    }
}
