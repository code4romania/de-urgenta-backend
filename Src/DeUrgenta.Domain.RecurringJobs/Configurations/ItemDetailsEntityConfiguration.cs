using DeUrgenta.Domain.RecurringJobs.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.Domain.RecurringJobs.Configurations
{
    public class ItemDetailsEntityConfiguration : IEntityTypeConfiguration<ItemDetails>
    {
        public void Configure(EntityTypeBuilder<ItemDetails> builder)
        {
            builder.HasKey(i => i.NotificationId)
                .HasName("PK_Item");
        }
    }
}
