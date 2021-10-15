using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeUrgenta.Domain.I18n.Entities
{
    public sealed class Language
    {
        public Language()
        {
            StringResources = new HashSet<StringResource>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Culture { get; set; }

        public ICollection<StringResource> StringResources { get; set; }
    }
}