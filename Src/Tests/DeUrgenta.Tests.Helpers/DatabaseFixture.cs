using System.IO;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DeUrgenta.Tests.Helpers
{
    public class DatabaseFixture
    {
        private readonly string _connectionString;

        public DeUrgentaContext Context { get; }

        public DatabaseFixture()
        {
            _connectionString = GetConnectionString();
            var optionsBuilder = new DbContextOptionsBuilder<DeUrgentaContext>();
            optionsBuilder.UseNpgsql(_connectionString);
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
    }
}