using DeUrgenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.Domain.Configurations
{
    public class InviteEntityConfiguration : IEntityTypeConfiguration<Invite>
    {
        public void Configure(EntityTypeBuilder<Invite> builder)
        {
            builder.HasKey(i => i.Id)
                .HasName("PK_Invite");

            builder.Property(i => i.DestinationId)
                .IsRequired();

            builder.Property(i => i.InviteStatus)
                .IsRequired()
                .HasDefaultValue(InviteStatus.Sent);

            builder.Property(i => i.Type)
                .IsRequired();

            builder.Property(i => i.SentOn)
                .IsRequired()
                .HasDefaultValueSql("current_date");
        }
    }
}
