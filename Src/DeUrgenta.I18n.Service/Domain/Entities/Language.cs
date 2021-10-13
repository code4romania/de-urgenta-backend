using System.Collections.Generic;

namespace DeUrgenta.I18n.Service.Domain.Entities
{
    public class Language
    {
        public Language()
        {
            StringResources = new HashSet<StringResource>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Culture { get; set; }

        public virtual ICollection<StringResource> StringResources { get; set; }
    }
}