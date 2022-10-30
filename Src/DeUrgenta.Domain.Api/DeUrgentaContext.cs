using System;
using DeUrgenta.Domain.Api.Configurations;
using DeUrgenta.Domain.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Domain.Api
{
    public class DeUrgentaContext : DbContext, IDatabaseContext
    {
        public DeUrgentaContext(DbContextOptions<DeUrgentaContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<Backpack> Backpacks { get; set; }
        public DbSet<BackpackItem> BackpackItems { get; set; }
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupSafeLocation> GroupsSafeLocations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLocation> UserLocations { get; set; }
        public DbSet<UserToGroup> UsersToGroups { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<BlogPost> Blogs { get; set; }
        public DbSet<BackpackToUser> BackpacksToUsers { get; set; }
        public DbSet<Invite> Invites { get; set; }

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

            modelBuilder.ApplyConfiguration(new EventTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new InviteEntityConfiguration());
        }

        public string SchemaName => "public";
    }

    public interface IDatabaseContext
    {
        string SchemaName { get; }
    }
}
