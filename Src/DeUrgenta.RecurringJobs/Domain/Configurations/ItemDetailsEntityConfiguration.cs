using DeUrgenta.RecurringJobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.RecurringJobs.Domain.Configurations
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
