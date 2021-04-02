using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeUrgenta.Domain.Entities
{
    public class BackpackToUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid BackpackId { get; set; }
        public Backpack Backpack { get; set; }
    }
}