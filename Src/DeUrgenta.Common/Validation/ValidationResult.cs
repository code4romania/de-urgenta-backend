using System.Collections.Immutable;

namespace DeUrgenta.Common.Validation
{
    public abstract record ValidationResult
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        
        public ImmutableDictionary<string, string> Messages { get; set; }

        public ValidationResult(bool isSuccess, ImmutableDictionary<string, string> messages)
        {
            IsSuccess = isSuccess;
            Messages = messages ?? ImmutableDictionary<string, string>.Empty;
        }

        public static ValidationResult Ok { get; } = new ValidationPassed();
        public static ValidationResult GenericValidationError { get; } = new GenericValidationError();
    }
}