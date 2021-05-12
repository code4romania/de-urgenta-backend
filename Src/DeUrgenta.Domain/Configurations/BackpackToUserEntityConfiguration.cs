using DeUrgenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.Domain.Configurations
{
    internal class BackpackToUserEntityConfiguration : IEntityTypeConfiguration<BackpackToUser>
    {
        public void Configure(EntityTypeBuilder<BackpackToUser> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("PK_BackpackToUser");

            builder
                .Property(e => e.IsOwner)
                .IsRequired();

            builder
                .HasIndex(e => e.UserId)
                .HasDatabaseName("IX_BackpackToUser_User");

            builder
                .HasIndex(e => e.BackpackId)
                .HasDatabaseName("IX_BackpackToUser_Backpack");

            builder
                .HasIndex(e => new { e.UserId, e.BackpackId })
                .HasDatabaseName("IX_BackpackToUser")
                .IsUnique();

            builder
                .HasIndex(e => new { e.BackpackId, HasOwner = e.IsOwner })
                .HasDatabaseName("IX_Backpack_Owner")
                .IsUnique();

            builder
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_BackpackToUser_User");

            builder
                .HasOne(d => d.Backpack)
                .WithMany()
                .HasForeignKey(d => d.BackpackId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_BackpackToUser_Backpack");
        }
    }
}