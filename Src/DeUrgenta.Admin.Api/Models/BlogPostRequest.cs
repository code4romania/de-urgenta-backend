using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Admin.Api.Models
{
    public sealed record BlogPostRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Author { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        public string ContentBody { get; set; }
    }
}
