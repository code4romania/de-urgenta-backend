using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.Api.Entities
{
    public class BlogPost
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ContentBody { get; set; }
        public DateTime PublishedOn { get; set; }
        public string Author { get; set; }
    }
}
