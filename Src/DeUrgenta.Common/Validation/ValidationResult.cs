namespace DeUrgenta.Common.Validation
{
    public abstract record ValidationResult
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public ValidationResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public static ValidationResult Ok { get; } = new ValidationPassed();
        public static ValidationResult GenericValidationError { get; } = new GenericValidationError();
    }
}