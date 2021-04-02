using DeUrgenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.Domain.Configurations
{
    internal class BackpackCategoryEntityConfiguration : IEntityTypeConfiguration<BackpackCategory>
    {
        public void Configure(EntityTypeBuilder<BackpackCategory> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("PK_BackpackCategory");

            builder
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");

            builder
                .Property(e => e.Name)
                .HasMaxLength(250)
                .IsRequired();

            builder.HasOne(d => d.Backpack)
                .WithMany(p => p.Categories)
                .HasForeignKey(d => d.BackpackId)
                .HasConstraintName("FK_Backpack_BackpackCategory");
        }
    }
}