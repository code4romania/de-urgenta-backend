
namespace DeUrgenta.Admin.Api.Models
{
    public sealed record BlogPostRequest
    {
        public string Author { get; set; }

        public string Title { get; set; }

        public string ContentBody { get; set; }
    }
}
