using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DeUrgenta.Api.Extensions;
using DeUrgenta.Backpack.Api.Controllers;
using System.Collections.Generic;
using System.Reflection;
using DeUrgenta.Certifications.Api.Controller;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using System.Linq;

namespace DeUrgenta.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            WebHostEnvironment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDatabase(Configuration.GetConnectionString("DbConnectionString"));
            services.AddExceptionHandling(WebHostEnvironment);


            var applicationAssemblies = GetAssemblies();

            services.AddSwaggerFor(applicationAssemblies);
            services.AddMediatR(applicationAssemblies.ToArray());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (WebHostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseProblemDetails();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeUrgenta.Api v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private IEnumerable<Assembly> GetAssemblies()
        {
            yield return Assembly.GetAssembly(typeof(Startup));

            // Application parts
            yield return typeof(BackpackController).GetTypeInfo().Assembly;
            yield return typeof(CertificationCotroller).GetTypeInfo().Assembly;
        }
    }
}
