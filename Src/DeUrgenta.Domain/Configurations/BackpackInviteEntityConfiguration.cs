using DeUrgenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.Domain.Configurations
{
    public class BackpackInviteEntityConfiguration : IEntityTypeConfiguration<BackpackInvite>
    {
        public void Configure(EntityTypeBuilder<BackpackInvite> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("PK_Backpack_invite");

            builder
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");

            builder
                .HasIndex(e => e.InvitationReceiverId)
                .HasDatabaseName("IX_BackpackInvite_InvitationReceiver");

            builder
                .HasIndex(e => e.InvitationSenderId)
                .HasDatabaseName("IX_BackpackInvite_InvitationSender");

            builder
                .HasIndex(e => e.BackpackId)
                .HasDatabaseName("IX_BackpackInvite_Backpack");

            builder.HasOne(d => d.InvitationSender)
                .WithMany()
                .HasForeignKey(d => d.InvitationSenderId)
                .HasConstraintName("FK_BackpackInvite_InvitationSender");

            builder.HasOne(d => d.InvitationReceiver)
                .WithMany()
                .HasForeignKey(d => d.InvitationReceiverId)
                .HasConstraintName("FK_BackpackInvite_InvitationReceiver");

            builder.HasOne(d => d.Backpack)
                .WithMany()
                .HasForeignKey(d => d.BackpackId)
                .HasConstraintName("FK_BackpackInvite_Backpack");


            builder
                .HasIndex(e => new { e.BackpackId, e.InvitationReceiverId, e.InvitationSenderId })
                .HasDatabaseName("IX_BackpackInvite")
                .IsUnique();
        }
    }
}