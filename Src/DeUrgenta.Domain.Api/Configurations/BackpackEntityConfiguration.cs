using DeUrgenta.Domain.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.Domain.Api.Configurations
{
    internal class BackpackEntityConfiguration : IEntityTypeConfiguration<Backpack>
    {
        public void Configure(EntityTypeBuilder<Backpack> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("PK_Backpack");

            builder
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");

            builder
                .Property(e => e.Name)
                .HasMaxLength(250)
                .IsRequired();
        }
    }
}
