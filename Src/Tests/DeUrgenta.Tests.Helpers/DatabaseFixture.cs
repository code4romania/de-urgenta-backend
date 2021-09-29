using System.Threading.Tasks;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;
using Xunit;

namespace DeUrgenta.Tests.Helpers
{
    public class DatabaseFixture : IAsyncLifetime
    {
        private readonly TestConfig _testConfig;
        private readonly Checkpoint _emptyDatabaseCheckpoint;
        public DeUrgentaContext Context { get; }

        public DatabaseFixture()
        {
            _testConfig = new TestConfig();

            var optionsBuilder = new DbContextOptionsBuilder<DeUrgentaContext>();
            optionsBuilder.UseNpgsql(_testConfig.ConnectionString);
            if (_testConfig.UseDbCheckpoint)
            {
                _emptyDatabaseCheckpoint = new Checkpoint
                {
                    SchemasToInclude = new[] { "public" },
                    TablesToIgnore = new[] { "__EFMigrationsHistory", "EventTypes" },
                    DbAdapter = DbAdapter.Postgres
                };
            }

            // Create instance of you application's DbContext
            Context = new DeUrgentaContext(optionsBuilder.Options);
            Context.Database.Migrate();
        }

        public async Task InitializeAsync()
        {
            if (!_testConfig.UseDbCheckpoint)
                return;

            await using var conn = new NpgsqlConnection(_testConfig.ConnectionString);
            await conn.OpenAsync();

            await _emptyDatabaseCheckpoint.Reset(conn);
        }

        public async Task DisposeAsync()
        {
            if (!_testConfig.UseDbCheckpoint)
                return;

            await using var conn = new NpgsqlConnection(_testConfig.ConnectionString);
            await conn.OpenAsync();

            await _emptyDatabaseCheckpoint.Reset(conn);
        }
    }
}