using System;

namespace DeUrgenta.Group.Api.Models
{
    public sealed record GroupModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public int NumberOfMembers { get; set; }
        public int MaxNumberOfMembers { get; set; }
        public Guid AdminId { get; set; }
        public string AdminFirstName { get; set; }
        public string AdminLastName { get; set; }
    }
}