using System.Collections.Immutable;

namespace DeUrgenta.Common.Validation
{
    public abstract record ValidationResult(bool IsSuccess, ImmutableDictionary<string, string> Messages)
    {
        public static ValidationResult Ok { get; } = new ValidationPassedResult();
        public static ValidationResult GenericValidationError { get; } = new GenericValidationError();

        public bool IsFailure { get; } = !IsSuccess;

        public ImmutableDictionary<string, string> Messages { get; } = Messages ?? ImmutableDictionary<string, string>.Empty;
    }
}