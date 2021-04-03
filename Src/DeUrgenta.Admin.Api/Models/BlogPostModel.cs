using System;

namespace DeUrgenta.Admin.Api.Models
{
    public sealed record BlogPostModel
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string ContentBody { get; init; }
        public DateTime PublishedOn { get; init; }
        public string Author { get; init; }
    }
}
