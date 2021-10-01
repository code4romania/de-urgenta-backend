using DeUrgenta.RecurringJobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.RecurringJobs.Domain.Configurations
{
    public class NotificationEntityConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("PK_Notification");

            builder
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(x => x.Type)
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasDefaultValue(NotificationStatus.NotSent);

            builder.Property(x => x.ScheduledDate)
                .IsRequired();
        }
    }
}
