namespace DeUrgenta.Common.Validation
{
    public record GenericValidationError : ValidationResult
    {
        public GenericValidationError() : base(false)
        {
        }
    }
}