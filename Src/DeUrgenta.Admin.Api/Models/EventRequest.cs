using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Admin.Api.Models
{
    public sealed record EventRequest
    {
        [Required]
        [StringLength(250, MinimumLength = 3)]
        public string Title { get; init; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string OrganizedBy { get; init; }

        [Required]
        public string ContentBody { get; init; }

        [Required]
        public DateTime OccursOn { get; init; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Author { get; init; }
    }
}
