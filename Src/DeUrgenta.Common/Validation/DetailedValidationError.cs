using System.Collections.Generic;
using System.Collections.Immutable;

namespace DeUrgenta.Common.Validation
{
    public record DetailedValidationError : ValidationResult
    {
        public DetailedValidationError(ImmutableDictionary<string, string> messages) : base(false, messages)
        {
        }

        public DetailedValidationError(Dictionary<string, string> messages) : base(false, messages.ToImmutableDictionary())
        {
        }

        public DetailedValidationError(string title, string message) : base(false, new Dictionary<string, string> { { title, message } }.ToImmutableDictionary())
        {
        }


    }
}