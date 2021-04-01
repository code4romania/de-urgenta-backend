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
                .HasIndex(e => e.UserId)
                .HasDatabaseName("IX_BackpackToUser_User");

            builder
                .HasIndex(e => e.BackpackId)
                .HasDatabaseName("IX_BackpackToUser_Backpack");

            builder
                .HasIndex(e => new {e.UserId, e.BackpackId})
                .HasDatabaseName("IX_BackpackToUser")
                .IsUnique();

            builder
                .HasOne(d => d.User)
                .WithMany(p => p.BackpackUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_BackpackToUser_User");

            builder
                .HasOne(d => d.Backpack)
                .WithMany(p => p.BackpackUsers)
                .HasForeignKey(d => d.BackpackId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_BackpackToUser_Backpack");
        }
    }
}