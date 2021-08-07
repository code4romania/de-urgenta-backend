using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.Entities
{
    public class UserLocation
    {
        [Key]
        public Guid Id { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public UserLocationType Type { get; set; }
        public virtual User User { get; set; }
    }
}