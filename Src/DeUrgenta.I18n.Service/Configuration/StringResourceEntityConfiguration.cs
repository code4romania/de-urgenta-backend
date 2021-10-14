using DeUrgenta.I18n.Service.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.I18n.Service.Configuration
{
    internal class StringResourceEntityConfiguration  : IEntityTypeConfiguration<StringResource>
    {
        public void Configure(EntityTypeBuilder<StringResource> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");

            builder
                .Property(e => e.Key)
                .HasMaxLength(250)
                .IsRequired();

            builder
                .Property(e => e.Value)
                .IsRequired();

            builder
                .HasIndex(x => new { x.LanguageId, Name = x.Key })
                .IsUnique();

            builder
                .HasOne(x => x.Language)
                .WithMany(x => x.StringResources)
                .HasForeignKey(x=>x.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}