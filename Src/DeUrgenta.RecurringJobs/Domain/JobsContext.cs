using DeUrgenta.RecurringJobs.Domain.Configurations;
using DeUrgenta.RecurringJobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.RecurringJobs.Domain
{
    public class JobsContext : DbContext
    {
        public JobsContext(DbContextOptions<JobsContext> options) : base(options)
        { }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<CertificationDetails> CertificationDetails { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.HasDefaultSchema("jobs");

            modelBuilder.ApplyConfiguration(new NotificationEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CertificationDetailsEntityConfiguration());
        }
    }
}
