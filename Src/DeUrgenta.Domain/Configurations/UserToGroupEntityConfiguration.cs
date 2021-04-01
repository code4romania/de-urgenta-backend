using DeUrgenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.Domain.Configurations
{
    internal class UserToGroupEntityConfiguration : IEntityTypeConfiguration<UserToGroup>
    {
        public void Configure(EntityTypeBuilder<UserToGroup> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("PK_UserToGroup");

            builder
                .HasIndex(e => e.UserId)
                .HasDatabaseName("IX_UserToGroup_User");

            builder
                .HasIndex(e => e.GroupId)
                .HasDatabaseName("IX_UserToGroup_Group");

            builder
                .HasIndex(e => new { e.UserId, e.GroupId })
                .HasDatabaseName("IX_UserToGroup")
                .IsUnique();

            builder
                .HasOne(d => d.User)
                .WithMany(p => p.GroupsMember)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_UserToGroup_User");

            builder
                .HasOne(d => d.Group)
                .WithMany(p => p.GroupMembers)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_UserToGroup_Group");
        }
    }
}