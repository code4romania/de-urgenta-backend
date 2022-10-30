using DeUrgenta.Domain.Api;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Tests.Helpers
{
    public class DatabaseFixture 
    {
        protected readonly TestConfig TestConfig;
        public DeUrgentaContext Context { get; }

        public DatabaseFixture()
        {
            TestConfig = new TestConfig();

            var optionsBuilder = new DbContextOptionsBuilder<DeUrgentaContext>();
            optionsBuilder.UseNpgsql(TestConfig.ConnectionString);
           
            // Create instance of you application's DbContext
            Context = new DeUrgentaContext(optionsBuilder.Options);
            Context.Database.Migrate();
        }
    }
}