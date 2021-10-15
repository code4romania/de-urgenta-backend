using System.Threading.Tasks;
using DeUrgenta.Domain.RecurringJobs;
using DeUrgenta.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;

namespace DeUrgenta.RecurringJobs.Tests
{
    public class JobsDatabaseFixture : DatabaseFixture
    {
        private readonly JobsTestConfig _testConfig;
        private readonly Checkpoint _emptyJobsDatabaseCheckpoint;

        public JobsContext JobsContext;

        public JobsDatabaseFixture()
        {
            _testConfig = new JobsTestConfig();
            var optionsBuilder = new DbContextOptionsBuilder<JobsContext>();
            optionsBuilder.UseNpgsql(_testConfig.JobsConnectionString);
            if (_testConfig.UseDbCheckpoint)
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

            if (!_testConfig.UseDbCheckpoint)
                return;

            await using var conn = new NpgsqlConnection(_testConfig.JobsConnectionString);
            await conn.OpenAsync();

            await _emptyJobsDatabaseCheckpoint.Reset(conn);
        }

        public new async Task DisposeAsync()
        {
            await base.DisposeAsync();

            if (!_testConfig.UseDbCheckpoint)
                return;

            await using var conn = new NpgsqlConnection(_testConfig.JobsConnectionString);
            await conn.OpenAsync();

            await _emptyJobsDatabaseCheckpoint.Reset(conn);
        }
    }
}
