using DeUrgenta.Domain.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.Domain.Api.Configurations
{
    internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("PK_User");

            builder
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");

            builder
                .Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(x => x.Sub)
                .IsRequired();

            builder
                .HasMany(x => x.Locations)
                .WithOne(l=> l.User);
        }
    }
}
