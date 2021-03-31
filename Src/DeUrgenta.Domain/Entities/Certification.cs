using System;

namespace DeUrgenta.Domain.Entities
{
    public class Certification
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int IdUser { get; set; }
        public virtual User User { get; set; }
    }
}
