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
        public DbSet<BackpackItem> BackpackItems { get; set; }
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<CourseCity> CourseCities { get; set; }
        public DbSet<CourseType> CourseTypes { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupSafeLocation> GroupsSafeLocations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLocation> UserLocations { get; set; }
        public DbSet<UserToGroup> UsersToGroups { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<BlogPost> Blogs { get; set; }
        public DbSet<GroupInvite> GroupInvites { get; set; }
        public DbSet<BackpackInvite> BackpackInvites { get; set; }

        public DbSet<BackpackToUser> BackpacksToUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.ApplyConfiguration(new BackpackEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BackpackItemEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BackpackToUserEntityConfiguration());

            modelBuilder.ApplyConfiguration(new CertificationEntityConfiguration());
            modelBuilder.ApplyConfiguration(new GroupEntityConfiguration());

            modelBuilder.ApplyConfiguration(new GroupSafeLocationEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserAddressEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());

            modelBuilder.ApplyConfiguration(new UserToGroupEntityConfiguration());

            modelBuilder.ApplyConfiguration(new BlogPostEntityConfiguration());
            modelBuilder.ApplyConfiguration(new EventEntityConfiguration());

            modelBuilder.ApplyConfiguration(new GroupInviteEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BackpackInviteEntityConfiguration());

            modelBuilder.ApplyConfiguration(new CourseCityEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CourseTypeEntityConfiguration());
        }
    }
}
