using System;

namespace DeUrgenta.Backpack.Api.Models
{
    public sealed record BackpackModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public int NumberOfContributors { get; set; }
        public int MaxNumberOfContributors { get; set; }
    }
}