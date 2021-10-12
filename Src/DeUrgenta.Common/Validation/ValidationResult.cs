using System.Collections.Immutable;

namespace DeUrgenta.Common.Validation
{
    public abstract record ValidationResult(bool IsSuccess, ImmutableList<string> Messages)
    {
        public static ValidationResult Ok { get; } = new ValidationPassed();
        public static ValidationResult GenericValidationError { get; } = new GenericValidationError();

        public bool IsFailure { get; } = !IsSuccess;

        public ImmutableList<string> Messages { get; init; } = Messages ?? ImmutableList<string>.Empty;
    }
}