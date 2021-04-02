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
        }

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid AdminId { get; set; }
        public User Admin { get; set; }
        public Guid SafeLocation1Id { get; set; }
        public GroupSafeLocation SafeLocation1 { get; set; }
        public Guid SafeLocation2Id { get; set; }
        public GroupSafeLocation SafeLocation2 { get; set; }
        public ICollection<UserToGroup> GroupMembers { get; set; }
    }
}