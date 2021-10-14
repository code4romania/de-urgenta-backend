using DeUrgenta.Domain.RecurringJobs.Configurations;
using DeUrgenta.Domain.RecurringJobs.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Domain.RecurringJobs
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
