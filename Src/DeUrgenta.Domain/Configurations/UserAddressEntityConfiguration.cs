using DeUrgenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.Domain.Configurations
{
    internal class UserAddressEntityConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("PK_UserAddress");

            builder
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");

            builder
                .Property(x => x.Name)
                .HasMaxLength(250)
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