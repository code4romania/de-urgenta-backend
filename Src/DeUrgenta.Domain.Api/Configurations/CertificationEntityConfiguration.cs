using DeUrgenta.Domain.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.Domain.Api.Configurations
{
    internal class CertificationEntityConfiguration : IEntityTypeConfiguration<Certification>
    {
        public void Configure(EntityTypeBuilder<Certification> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("PK_Certification");

            builder
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");

            builder
                .Property(x => x.Name)
                .HasMaxLength(250)
                .IsRequired();

            builder
                .Property(x => x.IssuingAuthority)
                .HasMaxLength(250)
                .IsRequired();

            builder
                .Property(x => x.ExpirationDate)
                .IsRequired();
            
            builder.HasOne(d => d.User)
                .WithMany(p => p.Certifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_User_Certification");
        }
    }
}
