using System.Collections.Immutable;

namespace DeUrgenta.Common.Validation
{
    public record ValidationPassedResult : ValidationResult
    {
        public ValidationPassedResult() : base(true, ImmutableDictionary<string, string>.Empty)
        {
        }
    }
}