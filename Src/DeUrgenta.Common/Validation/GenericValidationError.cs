using System.Collections.Immutable;

namespace DeUrgenta.Common.Validation
{
    public record GenericValidationError : ValidationResult
    {
        public GenericValidationError() : base(false, ImmutableDictionary<string, string>.Empty)
        {
        }
    }
}