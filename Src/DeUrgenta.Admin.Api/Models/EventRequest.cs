using System;

namespace DeUrgenta.Admin.Api.Models
{
    public sealed record EventRequest
    {
        public string Title { get; init; }

        public string OrganizedBy { get; init; }

        public string ContentBody { get; init; }

        public DateTime OccursOn { get; init; }

        public string Author { get; init; }

        public string City { get; init; }

        public int EventTypeId { get; init; }
        public string Address { get; init; }
    }
}
