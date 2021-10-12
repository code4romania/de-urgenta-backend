using System.Collections.Immutable;

namespace DeUrgenta.Common.Validation
{
    public record ValidationPassed : ValidationResult
    {
        public ValidationPassed() : base(true, ImmutableList.Create<string>())
        {
        }
    }
}