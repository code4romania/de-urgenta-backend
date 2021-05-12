using DeUrgenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.Domain.Configurations
{
    public class GroupInviteEntityConfiguration : IEntityTypeConfiguration<GroupInvite>
    {
        public void Configure(EntityTypeBuilder<GroupInvite> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("PK_Group_invite");

            builder
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");

            builder
                .HasIndex(e => e.InvitationReceiverId)
                .HasDatabaseName("IX_GroupInvite_InvitationReceiver");

            builder
                .HasIndex(e => e.InvitationSenderId)
                .HasDatabaseName("IX_GroupInvite_InvitationSender");

            builder
                .HasIndex(e => e.GroupId)
                .HasDatabaseName("IX_GroupInvite_Group");

            builder.HasOne(d => d.InvitationSender)
                .WithMany()
                .HasForeignKey(d => d.InvitationSenderId)
                .HasConstraintName("FK_GroupInvite_InvitationSender");

            builder.HasOne(d => d.InvitationReceiver)
                .WithMany()
                .HasForeignKey(d => d.InvitationReceiverId)
                .HasConstraintName("FK_GroupInvite_InvitationReceiver");

            builder.HasOne(d => d.Group)
                .WithMany()
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_GroupInvite_Group");


            builder
                .HasIndex(e => new { e.GroupId, e.InvitationReceiverId, e.InvitationSenderId })
                .HasDatabaseName("IX_GroupInvite")
                .IsUnique();
        }
    }
}