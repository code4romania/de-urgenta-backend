using System.Collections.Generic;
using System.Collections.Immutable;

namespace DeUrgenta.Common.Validation
{
    public record LocalizableValidationError : ValidationResult
    {
        public LocalizableValidationError(ImmutableDictionary<string, string> messages) : base(false, messages)
        {
        }

        public LocalizableValidationError(Dictionary<string, string> messages) : base(false, messages.ToImmutableDictionary())
        {
        }

        public LocalizableValidationError(string title, string message) : base(false, new Dictionary<string, string> { { title, message } }.ToImmutableDictionary())
        {
        }
    }
}