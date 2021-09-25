using System;
using DeUrgenta.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace DeUrgenta.Specs
{
    public class ApiWebApplicationFactory
    {
        private readonly TestServer _testServer;

        // Must be set in each test
        public ITestOutputHelper Output { get; set; }

        public ApiWebApplicationFactory(ITestOutputHelper output)
        {
            Output = output;

            _testServer = new TestServer(new WebHostBuilder()
                .ConfigureAppConfiguration((_, builder) =>
                {
                    builder.AddJsonFile("appsettings.specs.json");
                    builder.AddEnvironmentVariables();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders(); // Remove other loggers
                    logging.AddXUnit(Output); // Use the ITestOutputHelper instance
                })
                .UseStartup<Startup>());
        }

        public HttpClient CreateClient()
        {
            return _testServer.CreateClient();
        }
    }
}
