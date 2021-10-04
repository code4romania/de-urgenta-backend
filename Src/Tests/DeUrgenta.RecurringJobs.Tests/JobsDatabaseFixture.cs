using System.Threading.Tasks;
using DeUrgenta.RecurringJobs.Domain;
using DeUrgenta.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;

namespace DeUrgenta.RecurringJobs.Tests
{
    public class JobsDatabaseFixture : DatabaseFixture
    {
        private readonly Checkpoint _emptyJobsDatabaseCheckpoint;

        public JobsContext JobsContext;

        public JobsDatabaseFixture()
            : base()
        {
            var optionsBuilder = new DbContextOptionsBuilder<JobsContext>();
            optionsBuilder.UseNpgsql(TestConfig.JobsConnectionString);
            if (TestConfig.UseDbCheckpoint)
            {
                _emptyJobsDatabaseCheckpoint = new Checkpoint
                {
                    SchemasToInclude = new[] { "jobs" },
                    TablesToIgnore = new[] { "__EFMigrationsHistory" },
                    DbAdapter = DbAdapter.Postgres
                };
            }

            // Create instance of you application's DbContext
            JobsContext = new JobsContext(optionsBuilder.Options);
            JobsContext.Database.Migrate();
        }

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            if (!TestConfig.UseDbCheckpoint)
                return;

            await using var conn = new NpgsqlConnection(TestConfig.JobsConnectionString);
            await conn.OpenAsync();

            await _emptyJobsDatabaseCheckpoint.Reset(conn);
        }

        public new async Task DisposeAsync()
        {
            await base.DisposeAsync();

            if (!TestConfig.UseDbCheckpoint)
                return;

            await using var conn = new NpgsqlConnection(TestConfig.JobsConnectionString);
            await conn.OpenAsync();

            await _emptyJobsDatabaseCheckpoint.Reset(conn);
        }
    }
}
