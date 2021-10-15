using DeUrgenta.Domain.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.Domain.Api.Configurations
{
    internal class BlogPostEntityConfiguration : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("PK_BlogPost");

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
                .Property(e => e.ContentBody)
                .IsRequired();

            builder
                .Property(e => e.PublishedOn)
                .IsRequired();
        }
    }
}
