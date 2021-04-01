using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.Entities
{
    public class BackpackCategory
    {
        public BackpackCategory()
        {
            BackpackItems = new HashSet<BackpackItem>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid BackpackId { get; set; }
        public Backpack Backpack { get; set; }
        public ICollection<BackpackItem> BackpackItems { get; set; }
    }
}