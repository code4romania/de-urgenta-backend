using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Extensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.I18n.Service.Providers;
using Microsoft.AspNetCore.Mvc;

namespace DeUrgenta.Common
{
    public class ResultMapper : IResultMapper
    {
        private readonly IamI18nProvider _i18nProvider;

        public ResultMapper(IamI18nProvider i18NProvider)
        {
            _i18nProvider = i18NProvider;
        }

        public async Task<ActionResult> MapToActionResult<T>(Result<T, ValidationResult> result)
        {
            if (result.IsFailure && result.Error is LocalizableValidationError)
            {
                var translatedErrorMessages = new Dictionary<string, string>();

                foreach (var errorMessage in result.Error.Messages)
                {
                    var translatedKey = await _i18nProvider.Localize(errorMessage.Key);
                    var translatedValue = await _i18nProvider.Localize(errorMessage.Value);

                    translatedErrorMessages.Add(translatedKey, translatedValue);
                }

                result.Error.Messages = translatedErrorMessages.ToImmutableDictionary();
            }

            return result.ToActionResult();
        }
    }
}
