using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeUrgenta.Domain.I18n.Entities
{
    public sealed class StringResource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public Guid LanguageId { get; set; }
        public Language Language { get; set; }
    }
}