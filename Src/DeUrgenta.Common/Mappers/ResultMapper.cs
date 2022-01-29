using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Extensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.I18n.Service.Providers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeUrgenta.Common.Mappers
{
    public class ResultMapper : IResultMapper
    {
        private readonly IamI18nProvider _i18nProvider;

        public ResultMapper(IamI18nProvider i18NProvider)
        {
            _i18nProvider = i18NProvider;
        }

        public async Task<ActionResult<T>> MapToActionResult<T>(Result<T, ValidationResult> result)
        {
           result = await TranslateResult(result);

            return result.ToActionResult();
        }

        private async Task<Result<T, ValidationResult>> TranslateResult<T>(Result<T, ValidationResult> result)
        {
            if (result.IsFailure && result.Error is LocalizableValidationError error)
            {
                var translatedErrorMessages = new Dictionary<string, string>();

                foreach (var errorMessage in error.Messages)
                {
                    var translatedKey = await _i18nProvider.Localize(errorMessage.Key);
                    var translatedValue = await _i18nProvider.Localize(errorMessage.Value);

                    translatedErrorMessages.Add(translatedKey, translatedValue);
                }

                return Result
                    .Failure<T, ValidationResult>(new DetailedValidationError(translatedErrorMessages));
            }

            return result;
        }

        public async Task<ActionResult> MapToActionResult(Result<Unit, ValidationResult> result)
        {
            result = await TranslateResult(result);

            return result.ToActionResult();
        }
    }
}
