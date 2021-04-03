using System.Collections.Immutable;

namespace DeUrgenta.Common.Models
{
    public record PagedResult<T> : PagedResultBase where T : class
    {
        public IImmutableList<T> Results { get; init; } = ImmutableList<T>.Empty;
    }
}