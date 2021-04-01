using DeUrgenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.Domain.Configurations
{
    internal class GroupSafeLocationEntityConfiguration : IEntityTypeConfiguration<GroupSafeLocation>
    {
        public void Configure(EntityTypeBuilder<GroupSafeLocation> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("PK_GroupSafeLocation");

            builder
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");

            builder
                .Property(x => x.Name)
                .HasMaxLength(250)
                .IsRequired();           
            
            builder
                .Property(x => x.Latitude)
                .IsRequired();

            builder
                .Property(x => x.Longitude)
                .IsRequired();
        }
    }

}