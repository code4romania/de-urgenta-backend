using DeUrgenta.Domain.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.Domain.Api.Configurations
{
    internal class EventEntityConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("PK_Event");

            builder
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");

            builder
                .Property(e => e.Title)
                .HasMaxLength(250)
                .IsRequired();

            builder
                .Property(e => e.Author)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(e => e.OrganizedBy)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(e => e.ContentBody)
                .IsRequired();

            builder
                .Property(e => e.PublishedOn)
                .IsRequired();

            builder
                .Property(e => e.OccursOn)
                .IsRequired();

            builder
                .Property(e => e.IsArchived)
                .IsRequired();

            builder
                .Property(e => e.Locality)
                .IsRequired();

            builder
                .HasIndex(e => e.Locality)
                .HasDatabaseName("IX_Event_City");

            builder
                .Property(e => e.Address)
                .IsRequired();

            builder
                .HasOne(d => d.EventType)
                .WithOne()
                .HasForeignKey<Event>(d => d.EventTypeId);
        }

    }
}