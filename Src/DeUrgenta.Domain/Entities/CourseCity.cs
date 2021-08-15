using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.Entities
{
    public class CourseCity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
