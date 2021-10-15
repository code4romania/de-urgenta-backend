using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeUrgenta.Domain.Api.Entities
{
    public class UserToGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public Guid GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}