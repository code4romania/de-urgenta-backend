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

        public DbSet<Backpack> Backpacks { get; set; }
        public DbSet<BackpackCategory> BackpackCategories { get; set; }
        public DbSet<BackpackToUser> BackpacksToUsers { get; set; }
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupSafeLocation> GroupsSafeLocations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserToGroup> UsersToGroups { get; set; }
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.ApplyConfiguration(new BackpackCategoryEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BackpackEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BackpackItemEntityConfiguration());

            modelBuilder.ApplyConfiguration(new BackpackToUserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CertificationEntityConfiguration());
            modelBuilder.ApplyConfiguration(new GroupEntityConfiguration());

            modelBuilder.ApplyConfiguration(new GroupSafeLocationEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserAddressEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());

            modelBuilder.ApplyConfiguration(new UserToGroupEntityConfiguration());
        }
    }
}
