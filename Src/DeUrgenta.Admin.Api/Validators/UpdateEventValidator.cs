using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.I18n.Service.Providers;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.Validators
{
    public class UpdateEventValidator : IValidateRequest<UpdateEvent>
    {
        private readonly DeUrgentaContext _context;
        private readonly IamI18nProvider _i18nProvider;

        public UpdateEventValidator(DeUrgentaContext context, IamI18nProvider i18NProvider)
        {
            _context = context;
            _i18nProvider = i18NProvider;
        }

        public async Task<ValidationResult> IsValidAsync(UpdateEvent request)
        {
            var eventExists = await _context.Events.AnyAsync(x => x.Id == request.EventId);
            if (!eventExists)
            {
                return new DetailedValidationError(await _i18nProvider.Localize("event-not-exist"), await _i18nProvider.Localize("event-not-exist-message", request.EventId));
            }

            var eventTypeExists = await _context.EventTypes.AnyAsync(x => x.Id == request.Event.EventTypeId);

            return eventTypeExists ? ValidationResult.Ok : new DetailedValidationError(await _i18nProvider.Localize("event-type-not-exist"), await _i18nProvider.Localize("event-type-not-exist-message", request.Event.EventTypeId));
        }
    }
}
