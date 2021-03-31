using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeUrgenta.Domain.Entities;

namespace DeUrgenta.Domain.Configurations
{
    internal class CertificationEntityConfiguration : IEntityTypeConfiguration<Certification>
    {
        public void Configure(EntityTypeBuilder<Certification> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("PK_Certification");

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(250);

            builder
                .Property(x => x.ExpirationDate)
                .IsRequired();

            builder.HasOne(d => d.User)
                .WithMany(p => p.Certifications)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_User_Certification");
        }
    }
}
