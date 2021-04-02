using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Backpack.Api.Models
{
    public sealed record BackpackItemRequest
    {
        [Required]
        [StringLength(250, MinimumLength = 3)]
        public string Name { get; init; }

        [Required]
        [Range(1, 99999)]
        public uint Amount { get; init; }

        [Required]
        [DataType(DataType.Date)] 
        public DateTime ExpirationDate { get; init; }
    }
}