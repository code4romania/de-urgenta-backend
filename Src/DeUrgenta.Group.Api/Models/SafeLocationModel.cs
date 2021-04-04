using System;

namespace DeUrgenta.Group.Api.Models
{
    public sealed record SafeLocationModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public decimal Latitude { get; init; }
        public decimal Longitude { get; init; }
    }
}
