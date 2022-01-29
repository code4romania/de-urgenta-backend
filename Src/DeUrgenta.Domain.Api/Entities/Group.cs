using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.Api.Entities
{
    public class Group
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid AdminId { get; set; }
        public virtual User Admin { get; set; }
        public virtual ICollection<UserToGroup> GroupMembers { get; set; } = new List<UserToGroup>();
        public virtual ICollection<GroupSafeLocation> GroupSafeLocations { get; set; } = new List<GroupSafeLocation>();
        public virtual Backpack Backpack { get; set; }
    }
}