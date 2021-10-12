using System.Collections.Immutable;

namespace DeUrgenta.Common.Validation
{
    public record DetailedValidationError : ValidationResult
    {
        public DetailedValidationError(ImmutableList<string> messages) : base(false, messages)
        {
        }
    }
}