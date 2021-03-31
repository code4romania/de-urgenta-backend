using Microsoft.EntityFrameworkCore;
using DeUrgenta.Domain.Configurations;
using DeUrgenta.Domain.Entities;

namespace DeUrgenta.Domain
{
    public class DeUrgentaContext : DbContext
    {
        public DeUrgentaContext(DbContextOptions<DeUrgentaContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Backpack> Backpacks { get; set; }
        public DbSet<Certification> Certifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BackpackEntityConfiguration());
        }
    }
}
