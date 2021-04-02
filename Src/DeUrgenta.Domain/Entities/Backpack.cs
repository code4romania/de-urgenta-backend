using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.Entities
{
    public class Backpack
    {
        public Backpack()
        {
            Categories = new HashSet<BackpackCategory>();
            BackpackUsers = new HashSet<BackpackToUser>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid AdminUserId { get; set; }
        public User AdminUser { get; set; }

        public ICollection<BackpackCategory> Categories { get; set; }
        public ICollection<BackpackToUser> BackpackUsers { get; set; }

    }
}
