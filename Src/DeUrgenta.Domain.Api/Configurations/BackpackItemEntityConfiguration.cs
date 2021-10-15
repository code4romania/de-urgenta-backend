using DeUrgenta.Domain.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.Domain.Api.Configurations
{
    internal class BackpackItemEntityConfiguration : IEntityTypeConfiguration<BackpackItem>
    {
        public void Configure(EntityTypeBuilder<BackpackItem> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("PK_BackpackItem");

            builder
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");

            builder
                .Property(e => e.Name)
                .HasMaxLength(250)
                .IsRequired();

            builder
                .Property(e => e.Amount)
                .IsRequired();

            builder
                .Property(e => e.ExpirationDate);

            builder
                .Property(e => e.BackpackCategory)
                .IsRequired();
            
            builder
                .HasOne(e => e.Backpack)
                .WithMany(x => x.BackpackItems)
                .HasForeignKey(x => x.BackpackId)
                .HasConstraintName("FK_BackpackItem_Backpack");
        }
    }
}