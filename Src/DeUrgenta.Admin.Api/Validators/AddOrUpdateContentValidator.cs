using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.I18n.Service.Providers;

namespace DeUrgenta.Admin.Api.Validators
{
    public class AddOrUpdateContentValidator : IValidateRequest<AddOrUpdateContent>
    {
        private readonly IAmLanguageProvider _languageProvider;
        private readonly IamI18nProvider _i18nProvider;

        public AddOrUpdateContentValidator(IAmLanguageProvider languageProvider, IamI18nProvider i18nProvider)
        {
            _languageProvider = languageProvider;
            _i18nProvider = i18nProvider;
        }

        public async Task<ValidationResult> IsValidAsync(AddOrUpdateContent request)
        {

            var language = await _languageProvider.GetLanguageByCulture(request.Culture);

            return language != null
                ? ValidationResult.Ok
                : new DetailedValidationError(await _i18nProvider.Localize("language-culture-not-exist"), await _i18nProvider.Localize("language-culture-not-exist-message", request.Culture));
        }
    }
}