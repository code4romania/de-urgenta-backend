using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeUrgenta.Domain.Entities;

namespace DeUrgenta.Domain.Configurations
{
    internal class EventTypeEntityConfiguration : IEntityTypeConfiguration<EventType>
    {
        public void Configure(EntityTypeBuilder<EventType> builder)
        {
            builder.ToTable("EventTypes");

            builder
                .HasKey(x => x.Id)
                .HasName("PK_EventType");

            builder
                .Property(x => x.Name)
                .HasMaxLength(250)
                .IsRequired();

            builder.HasData(
                new EventType { Id = 1, Name = "Prim ajutor" },
                new EventType { Id = 2, Name = "Prim ajutor calificat" },
                new EventType { Id = 3, Name = "Pregătire in caz de dezastre" }
            );
        }
    }
}
