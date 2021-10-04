﻿using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DeUrgenta.Tests.Helpers
{
    public class TestConfig
    {
        public string ConnectionString { get; set; }
        public string JobsConnectionString { get; set; }
        public bool UseDbCheckpoint { get; set; }

        public TestConfig()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.testing.json")
                .AddEnvironmentVariables()
                .Build();

            ConnectionString = configuration.GetConnectionString("TestingDbConnectionString");
            JobsConnectionString = configuration.GetConnectionString("JobsTestingDbConnectionString");
            UseDbCheckpoint = Convert.ToBoolean(configuration.GetSection("UseDbCheckpoint").Value);
        }
    }
}