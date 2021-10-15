using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeUrgenta.Domain.Api.Entities
{
    public class BackpackToUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public bool IsOwner { get; set; }
        public Guid BackpackId { get; set; }
        public virtual Backpack Backpack { get; set; }
    }
}