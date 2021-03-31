using System.Collections.Generic;

namespace DeUrgenta.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Certification> Certifications { get; set; }
    }
}
