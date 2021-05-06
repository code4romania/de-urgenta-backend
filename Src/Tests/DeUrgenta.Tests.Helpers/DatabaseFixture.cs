using System.IO;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Respawn;
using Xunit;

namespace DeUrgenta.Tests.Helpers
{
     public class DatabaseFixture : IAsyncLifetime
    {
        private readonly string _connectionString;

        private readonly Checkpoint _emptyDatabaseCheckpoint;
        public DeUrgentaContext Context { get; }

        public DatabaseFixture()
        {
            _connectionString = GetConnectionString();
            var optionsBuilder = new DbContextOptionsBuilder<DeUrgentaContext>();
            optionsBuilder.UseNpgsql(_connectionString);
            _emptyDatabaseCheckpoint = new Checkpoint
            {
                SchemasToInclude = new[] { "public" },
                TablesToIgnore = new[] { "__EFMigrationsHistory" },
                DbAdapter = DbAdapter.Postgres
            };

            // Create instance of you application's DbContext
            Context = new DeUrgentaContext(optionsBuilder.Options);
            Context.Database.Migrate();
        }

        private static string GetConnectionString()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.testing.json")
                .AddEnvironmentVariables()
                .Build();

            return configuration.GetConnectionString("TestingDbConnectionString");
        }

        public async Task InitializeAsync()
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await _emptyDatabaseCheckpoint.Reset(conn);
        }

        public async Task DisposeAsync()
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await _emptyDatabaseCheckpoint.Reset(conn);
        }
    }
}
