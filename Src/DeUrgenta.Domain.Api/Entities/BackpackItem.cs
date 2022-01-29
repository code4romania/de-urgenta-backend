using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.Api.Entities
{
    public class BackpackItem
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public uint Amount { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public BackpackCategoryType BackpackCategory { get; set; }
        public virtual Backpack Backpack { get; set; }
        public virtual Guid BackpackId { get; set; }
        public ulong Version { get; set; }
    }
}