using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.Entities
{
    public class Backpack
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Group Group { get; set; }
        public Guid GroupId { get; set; }
    }
}
