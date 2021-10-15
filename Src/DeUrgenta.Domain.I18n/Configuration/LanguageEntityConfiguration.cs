using System;
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
                   Id = Guid.Parse("a2f64834-36d3-4a94-84f2-33aac1a61ae7"),
                   Name = "English",
                   Culture = "en-US"
               },
               new Language
               {
                   Id = Guid.Parse("d0cb5c04-cb67-48c5-9252-6f6361a30a27"),
                   Name = "Romanian",
                   Culture = "ro-RO"
               },
               new Language
               {
                   Id = Guid.Parse("5959e6ce-8745-40bc-9806-ca3cd6731c6d"),
                   Name = "Hungarian",
                   Culture = "hu-HU"
               }
           );
        }
    }
}