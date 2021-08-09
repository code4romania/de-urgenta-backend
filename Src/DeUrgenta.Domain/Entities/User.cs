using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sub { get; set; }
        public virtual ICollection<Certification> Certifications { get; set; } = new List<Certification>();
        public virtual ICollection<Group> GroupsAdministered { get; set; } = new List<Group>();
        public virtual ICollection<UserToGroup> GroupsMember { get; set; } = new List<UserToGroup>();
        public virtual ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();
        public virtual ICollection<Course> FirstAidCourses { get; set; } = new List<Course>();
    }
}
