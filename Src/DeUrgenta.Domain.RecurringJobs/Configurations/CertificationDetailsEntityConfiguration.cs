using DeUrgenta.Domain.RecurringJobs.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.Domain.RecurringJobs.Configurations
{
    public class CertificationDetailsEntityConfiguration : IEntityTypeConfiguration<CertificationDetails>
    {
        public void Configure(EntityTypeBuilder<CertificationDetails> builder)
        {
            builder
                .HasKey(x => x.NotificationId)
                .HasName("PK_Certification");
        }
    }
}
