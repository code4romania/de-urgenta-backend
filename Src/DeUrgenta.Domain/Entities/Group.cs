using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.Entities
{
    public class Group
    {
        public Group()
        {
            GroupMembers = new HashSet<UserToGroup>();
            GroupSafeLocations = new HashSet<GroupSafeLocation>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid AdminId { get; set; }
        public User Admin { get; set; }
        public ICollection<UserToGroup> GroupMembers { get; set; }
        public ICollection<GroupSafeLocation> GroupSafeLocations { get; set; }
        public Backpack Backpack { get; set; }
    }
}