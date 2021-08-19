using DeUrgenta.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;

namespace DeUrgenta.Specs
{
    public class ApiWebApplicationFactory
    {
        private readonly TestServer _testServer;

        public ApiWebApplicationFactory()
        {
            _testServer = new TestServer(new WebHostBuilder()
                .ConfigureAppConfiguration((_, builder) =>
                {
                    builder.AddJsonFile("appsettings.specs.json");
                    builder.AddEnvironmentVariables();
                })
                .UseStartup<Startup>());
        }

        public HttpClient CreateClient()
        {
            return _testServer.CreateClient();
        }
    }
}
