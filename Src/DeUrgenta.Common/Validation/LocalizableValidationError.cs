using System.Collections.Generic;
using System.Collections.Immutable;
using DeUrgenta.I18n.Service.Models;

namespace DeUrgenta.Common.Validation
{
    public record LocalizableValidationError : ValidationResult
    {
        public ImmutableDictionary<LocalizableString, LocalizableString> Messages { get; }

        public LocalizableValidationError(ImmutableDictionary<LocalizableString, LocalizableString> messages) : base(false)
        {
            Messages = messages ?? ImmutableDictionary<LocalizableString, LocalizableString>.Empty;
        }

        public LocalizableValidationError(Dictionary<LocalizableString, LocalizableString> messages) : base(false)
        {

            Messages = messages?.ToImmutableDictionary() ?? ImmutableDictionary<LocalizableString, LocalizableString>.Empty;
        }

        public LocalizableValidationError(LocalizableString title, LocalizableString message) : base(false)
        {
            Messages = new Dictionary<LocalizableString, LocalizableString> { { title, message } }.ToImmutableDictionary();
        }
    }
}