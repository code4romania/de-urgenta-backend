using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.I18n.Service.Providers;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.Validators
{
    public class DeleteEventValidator : IValidateRequest<DeleteEvent>
    {
        private readonly DeUrgentaContext _context;
        private readonly IamI18nProvider _i18nProvider;

        public DeleteEventValidator(DeUrgentaContext context, IamI18nProvider i18nProvider)
        {
            _context = context;
            _i18nProvider = i18nProvider;
        }

        public async Task<ValidationResult> IsValidAsync(DeleteEvent request)
        {
            var @event = await _context.Events.FirstOrDefaultAsync(x => x.Id == request.EventId);

            if (@event == null)
            {
                return new DetailedValidationError(await _i18nProvider.Localize("event-not-exist"), await _i18nProvider.Localize("event-not-exist-message", request.EventId));
            }

            return ValidationResult.Ok;
        }
    }
}
