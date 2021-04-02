using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.Entities
{
    public class UserAddress
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}