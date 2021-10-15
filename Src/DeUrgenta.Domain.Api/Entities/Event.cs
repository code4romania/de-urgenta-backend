using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.Api.Entities
{
    public class Event
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string OrganizedBy { get; set; }
        public string ContentBody { get; set; }
        public DateTime OccursOn { get; set; }
        public string Author { get; set; }
        public DateTime PublishedOn { get; set; }
        public bool IsArchived { get; set; }

        public string City { get; set; }
        public string Address { get; set; }
        public int EventTypeId { get; set; }
        public virtual EventType EventType { get; set; }
    }
}
