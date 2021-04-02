using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.Entities
{
    public class BackpackItem
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public uint Amount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Guid BackpackCategoryId { get; set; }
        public BackpackCategory BackpackCategory { get; set; }
    }
}