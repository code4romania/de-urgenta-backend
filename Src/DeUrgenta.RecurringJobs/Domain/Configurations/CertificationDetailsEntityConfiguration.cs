using DeUrgenta.RecurringJobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.RecurringJobs.Domain.Configurations
{
    public class CertificationDetailsEntityConfiguration : IEntityTypeConfiguration<CertificationDetails>
    {
        public void Configure(EntityTypeBuilder<CertificationDetails> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("PK_Certification");
        }
    }
}
