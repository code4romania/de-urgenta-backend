using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.I18n.Service.Providers;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Events.Api.Validators
{
    public class GetEventValidator : IValidateRequest<GetEvent>
    {
        private readonly DeUrgentaContext _context;
        private readonly IamI18nProvider _i18nProvider;

        public GetEventValidator(DeUrgentaContext context, IamI18nProvider i18nProvider)
        {
            _context = context;
            _i18nProvider = i18nProvider;
        }

        public async Task<ValidationResult> IsValidAsync(GetEvent request)
        {
            var eventTypeExists = await _context.EventTypes.AnyAsync(x => x.Id == request.Filter.EventTypeId);

            return eventTypeExists ? ValidationResult.Ok : new DetailedValidationError(await _i18nProvider.Localize("event-type-not-exist"), await _i18nProvider.Localize("event-type-not-exist-message", request.Filter.EventTypeId));
        }
    }
}
