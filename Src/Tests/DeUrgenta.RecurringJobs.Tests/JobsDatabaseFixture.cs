using DeUrgenta.Domain.RecurringJobs;
using DeUrgenta.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.RecurringJobs.Tests
{
    public class JobsDatabaseFixture : DatabaseFixture
    {
        public JobsContext JobsContext;

        public JobsDatabaseFixture()
        {
            var testConfig = new JobsTestConfig();
            var optionsBuilder = new DbContextOptionsBuilder<JobsContext>();
            optionsBuilder.UseNpgsql(testConfig.JobsConnectionString);
          
            // Create instance of you application's DbContext
            JobsContext = new JobsContext(optionsBuilder.Options);
            JobsContext.Database.Migrate();
        }
    }
}
