using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.I18n.Service.Models;
using DeUrgenta.I18n.Service.Providers;

namespace DeUrgenta.Admin.Api.Validators
{
    public class AddOrUpdateContentValidator : IValidateRequest<AddOrUpdateContent>
    {
        private readonly IAmLanguageProvider _languageProvider;

        public AddOrUpdateContentValidator(IAmLanguageProvider languageProvider)
        {
            _languageProvider = languageProvider;
        }

        public async Task<ValidationResult> IsValidAsync(AddOrUpdateContent request, CancellationToken ct)
        {

            var language = await _languageProvider.GetLanguageByCulture(request.Culture);

            return language != null
                ? ValidationResult.Ok
                : new LocalizableValidationError("language-culture-not-exist", new LocalizableString("language-culture-not-exist-message", request.Culture));
        }
    }
}