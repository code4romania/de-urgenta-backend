namespace DeUrgenta.Api
{
    using System.Reflection;
    using Backpack.Api.Controllers;
    using Certifications.Api.Controller;
    using Extensions;
    using Hellang.Middleware.ProblemDetails;
    using Infra.Extensions;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

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
            services.AddMediatR(applicationAssemblies);

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

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private Assembly[] GetAssemblies()
        {
            return new[]
            {
                Assembly.GetAssembly(typeof(Startup)),

                // Application parts
                typeof(BackpackController).GetTypeInfo().Assembly,
                typeof(CertificationCotroller).GetTypeInfo().Assembly
            };
        }
    }
}
