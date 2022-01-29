using System.Threading.Tasks;
using DeUrgenta.Domain.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DeUrgenta.Tests.Helpers
{
    public class DatabaseFixture
    {
        private readonly Checkpoint _emptyDatabaseCheckpoint;

        protected readonly TestConfig TestConfig;
        public DeUrgentaContext Context { get; }

        public DatabaseFixture()
        {
            TestConfig = new TestConfig();

            var optionsBuilder = new DbContextOptionsBuilder<DeUrgentaContext>();
            optionsBuilder.UseNpgsql(TestConfig.ConnectionString);
            if (TestConfig.UseDbCheckpoint)
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
            if (!TestConfig.UseDbCheckpoint)
                return;

            await using var conn = new NpgsqlConnection(TestConfig.ConnectionString);
            await conn.OpenAsync();

            await _emptyDatabaseCheckpoint.Reset(conn);
        }

        public async Task DisposeAsync()
        {
            if (!TestConfig.UseDbCheckpoint)
                return;

            await using var conn = new NpgsqlConnection(TestConfig.ConnectionString);
            await conn.OpenAsync();

            return configuration.GetConnectionString("TestingDbConnectionString");
        }
    }
}