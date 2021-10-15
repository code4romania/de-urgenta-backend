using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Polly;

namespace DeUrgenta.Domains.Migrator
{
    public static class DbContextExtensions
    {
        public static async Task CreateAndMigrateAsync(this DbContext context)
        {
            var retryPolicy = Policy
                .Handle<PostgresException>(e => e.SqlState != PostgresErrorCodes.InvalidPassword && e.SqlState != PostgresErrorCodes.InvalidAuthorizationSpecification)
                .Or<ArgumentNullException>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: _ => TimeSpan.FromSeconds(2),
                    onRetry: (ex, _) =>
                        Console.WriteLine("Waiting to retry: Create and migrate database exception {0} with message: {1}", ex.GetType().FullName, ex.Message));

            try
            {
                await retryPolicy.ExecuteAsync(async () =>
                {
                    var currentTimeout = context.Database.GetCommandTimeout();
                    context.Database.SetCommandTimeout(currentTimeout * 10);
                    await context.Database.MigrateAsync();
                    context.Database.SetCommandTimeout(currentTimeout);
                });
            }
            catch (PostgresException ex)
            {
                Console.WriteLine($"PostgresException exception when creating/migrating database {ex.SqlState}.");
                throw;
            }
        }
    }
}