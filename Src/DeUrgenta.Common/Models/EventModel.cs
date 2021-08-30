using System;

namespace DeUrgenta.Common.Models
{
    public sealed record EventModel
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string OrganizedBy { get; init; }
        public string ContentBody { get; init; }
        public DateTime OccursOn { get; init; }
        public string Author { get; init; }
        public DateTime PublishedOn { get; init; }
        public bool IsArchived { get; init; }
        public string City { get; init; }
        public string Address { get; init; }
        public int EventTypeId { get; init; }
        public string EventType { get; init; }
    }
}
