using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.Entities
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string IssuingAuthority { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
