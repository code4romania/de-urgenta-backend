using System;
using Figgle;
using System.IO;
using System.Threading.Tasks;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Identity;
using DeUrgenta.Domain.I18n;
using DeUrgenta.Domain.RecurringJobs;
using DeUrgenta.Infra.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            Console.WriteLine(FiggleFonts.Ogre.Render("Domains Migrator"));
            Console.WriteLine("Registering contexts.");
            services.AddDatabase<DeUrgentaContext>(configuration.GetConnectionString("DbConnectionString"));
            services.AddDatabase<UserDbContext>(configuration.GetConnectionString("IdentityDbConnectionString"));
            services.AddDatabase<JobsContext>(configuration.GetConnectionString("I18nDbConnectionString"));
            services.AddDatabase<I18nDbContext>(configuration.GetConnectionString("JobsConnectionString"));
            Console.WriteLine("Done: Registering contexts.");

            var serviceProvider = services.BuildServiceProvider();

            Console.WriteLine("Getting contexts");

            var dbContexts = new DbContext[]
            {
                serviceProvider.GetService<DeUrgentaContext>(),
                serviceProvider.GetService<UserDbContext>(),
                serviceProvider.GetService<JobsContext>()
            };
            Console.WriteLine("Done: Getting contexts");
            Console.WriteLine("Applying migrations");

            foreach (var dbContext in dbContexts)
            {
                Console.WriteLine($"Migrating {dbContext.GetType().Name}.");
                await dbContext.CreateAndMigrateAsync();
                Console.WriteLine($"Done: Migrating {dbContext.GetType().Name}.");
                Console.WriteLine("--------------------------------------------");
            }

            Console.WriteLine("All good. Have a nice day!");
        }
    }
}
