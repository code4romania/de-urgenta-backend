using System;

namespace DeUrgenta.Group.Api.Models
{
    public sealed record GroupModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public int NumberOfMembers { get; init; }
        public int MaxNumberOfMembers { get; init; }
        public bool IsCurrentUserAdmin { get; init; }

    }
}