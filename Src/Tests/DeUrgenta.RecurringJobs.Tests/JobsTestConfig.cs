using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DeUrgenta.RecurringJobs.Tests
{
    public class JobsTestConfig
    {
        public string JobsConnectionString { get; set; }
        public bool UseDbCheckpoint { get; set; }

        public JobsTestConfig()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.testing.json")
                .AddEnvironmentVariables()
                .Build();

            JobsConnectionString = configuration.GetConnectionString("JobsTestingDbConnectionString");
            UseDbCheckpoint = Convert.ToBoolean(configuration.GetSection("UseDbCheckpoint").Value);
        }
    }
}