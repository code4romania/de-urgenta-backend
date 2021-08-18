using DeUrgenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.Domain.Configurations
{
    internal class UserAddressEntityConfiguration : IEntityTypeConfiguration<UserLocation>
    {
        public void Configure(EntityTypeBuilder<UserLocation> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("PK_UserAddress");

            builder
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");

            builder
                .Property(x => x.Address)
                .HasMaxLength(250)
                .IsRequired();

            builder
                .Property(x => x.Type)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(x => x.Latitude)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(x => x.Longitude)
                .IsRequired();
        }
    }
}