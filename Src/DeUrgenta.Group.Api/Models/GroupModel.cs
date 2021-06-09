using System;

namespace DeUrgenta.Group.Api.Models
{
    public sealed record GroupModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public bool IsAdmin { get; init; }
    }
}