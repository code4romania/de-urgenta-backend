using System.Collections.Generic;
using System.Collections.Immutable;

namespace DeUrgenta.Common.Validation
{
    public record DetailedValidationError : ValidationResult
    {
        public ImmutableDictionary<string, string> Messages { get; }

        public DetailedValidationError(ImmutableDictionary<string, string> messages) : base(false)
        {
            Messages = messages ?? ImmutableDictionary<string, string>.Empty;
        }

        public DetailedValidationError(Dictionary<string, string> messages) : base(false)
        {
            Messages = messages?.ToImmutableDictionary() ?? ImmutableDictionary<string, string>.Empty;
        }

        public DetailedValidationError(string title, string message) : base(false)
        {
            Messages = new Dictionary<string, string> { { title, message } }.ToImmutableDictionary();
        }
    }
}