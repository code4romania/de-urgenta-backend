using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeUrgenta.Domain.Entities;

namespace DeUrgenta.Domain.Configurations
{
    internal class CourseEntityConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
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
                .WithMany(p => p.Courses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_User_Certification");
        }
    }
}
