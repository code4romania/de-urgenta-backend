using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeUrgenta.Domain.Entities;

namespace DeUrgenta.Domain.Configurations
{
    internal class CourseCityEntityConfiguration : IEntityTypeConfiguration<CourseCity>
    {
        public void Configure(EntityTypeBuilder<CourseCity> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("PK_CourseCity");

            builder
                .Property(x => x.Name)
                .HasMaxLength(250)
                .IsRequired();
        }
    }
}
