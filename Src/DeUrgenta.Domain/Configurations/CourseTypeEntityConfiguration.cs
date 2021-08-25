using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeUrgenta.Domain.Entities;

namespace DeUrgenta.Domain.Configurations
{
    internal class CourseTypeEntityConfiguration : IEntityTypeConfiguration<CourseType>
    {
        public void Configure(EntityTypeBuilder<CourseType> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("PK_CourseType");

            builder
                .Property(x => x.Name)
                .HasMaxLength(250)
                .IsRequired();
        }
    }
}
