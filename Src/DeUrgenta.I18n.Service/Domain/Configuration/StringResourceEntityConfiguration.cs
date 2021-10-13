using DeUrgenta.I18n.Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeUrgenta.I18n.Service.Domain.Configuration
{
    public class StringResourceEntityConfiguration  : IEntityTypeConfiguration<StringResource>
    {
        public void Configure(EntityTypeBuilder<StringResource> builder)
        {
            
        }
    }
}