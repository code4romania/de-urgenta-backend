using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Backpack.Api.Models
{
    public sealed record BackpackModelRequest
    {
        [Required]
        [StringLength(250, MinimumLength = 3)]
        public string Name { get; init; }
    }
}