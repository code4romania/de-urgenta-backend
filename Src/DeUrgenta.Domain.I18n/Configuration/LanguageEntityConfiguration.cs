using DeUrgenta.Domain.I18n.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.Domain.I18n.Configuration
{
    internal class LanguageEntityConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");

            builder
                .Property(e => e.Name)
                .HasMaxLength(250)
                .IsRequired();

            builder
                .Property(e => e.Culture)
                .HasMaxLength(250)
                .IsRequired();

            builder
                .HasIndex(x => x.Culture)
                .IsUnique();

            builder.HasData(
               new Language
               {
                   Id = I18nDefaults.EnUsCultureId,
                   Name = "English",
                   Culture = "en-US"
               },
               new Language
               {
                   Id = I18nDefaults.RoRoCultureId,
                   Name = "Romanian",
                   Culture = "ro-RO"
               },
               new Language
               {
                   Id = I18nDefaults.HuHuCultureId,
                   Name = "Hungarian",
                   Culture = "hu-HU"
               }
           );
        }
    }
}