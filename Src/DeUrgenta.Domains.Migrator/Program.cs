using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Identity;
using DeUrgenta.Infra.Extensions;
using DeUrgenta.Domain.RecurringJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Colorful;

namespace DeUrgenta.Domains.Migrator
{

    public class Program
    {
        static async Task Main()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();

            Console.WriteAscii("Domains Migrator");
            Console.WriteLine("Registering contexts");

            services.AddDatabase<DeUrgentaContext>(configuration.GetConnectionString("DbConnectionString"));
            services.AddDatabase<UserDbContext>(configuration.GetConnectionString("IdentityDbConnectionString"));
            services.AddDatabase<JobsContext>(configuration.GetConnectionString("JobsConnectionString"));
            Console.WriteLine("==================================");

            var serviceProvider = services.BuildServiceProvider();

            Console.WriteLine("Getting contexts");

            var dbContexts = new DbContext[]
            {
                serviceProvider.GetService<DeUrgentaContext>(),
                serviceProvider.GetService<UserDbContext>(),
                serviceProvider.GetService<JobsContext>()
            };
            Console.WriteLine("==================================");

            Console.WriteLine("Applying migrations");

            foreach (var dbContext in dbContexts)
            {
                await dbContext.CreateAndMigrateAsync();
            }
            Console.WriteLine("==================================");
            Console.WriteLine("All good. Have a nice day!");

        }
    }
}
