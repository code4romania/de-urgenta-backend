using System.IO;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Respawn;
using Xunit;

namespace DeUrgenta.Group.Api.Tests
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

        private string GetConnectionString()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.testing.json", optional: false)
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

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}