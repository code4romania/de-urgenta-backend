using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.Api.Entities
{
    public class GroupSafeLocation
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public virtual Group Group { get; set; }
    }
}