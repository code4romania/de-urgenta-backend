using DeUrgenta.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace DeUrgenta.Specs
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private readonly IConfiguration _configuration;

        public ApiWebApplicationFactory()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.specs.json")
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                config.AddConfiguration(_configuration);
            });

        }
    }
}
