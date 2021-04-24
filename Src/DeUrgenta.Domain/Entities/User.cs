using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.Entities
{
    public class User
    {
        public User()
        {
            Backpacks = new List<Backpack>();
            Certifications = new List<Certification>();
            BackpackUsers = new List<BackpackToUser>();
            GroupsAdministered = new List<Group>();
            GroupsMember = new List<UserToGroup>();
            Addresses = new List<UserAddress>();
        }

        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sub { get; set; }
        public ICollection<Backpack> Backpacks { get; set; }
        public ICollection<Certification> Certifications { get; set; }
        public ICollection<BackpackToUser> BackpackUsers { get; set; }
        public ICollection<Group> GroupsAdministered { get; set; }
        public ICollection<UserToGroup> GroupsMember { get; set; }
        public ICollection<UserAddress> Addresses { get; set; }
    }
}
