using DeUrgenta.I18n.Service.Domain.Configuration;
using DeUrgenta.I18n.Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.I18n.Service.Domain
{
    public class I18nDbContext: DbContext
    {
        public DbSet<Language> Languages { get; set; }
        public DbSet<StringResource> StringResources { get; set; }


        public I18nDbContext(DbContextOptions<I18nDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.HasDefaultSchema("i18n");

            modelBuilder.ApplyConfiguration(new LanguageEntityConfiguration());
            modelBuilder.ApplyConfiguration(new StringResourceEntityConfiguration());

        }
    }
}