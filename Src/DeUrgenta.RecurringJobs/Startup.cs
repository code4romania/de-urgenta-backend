
using DeUrgenta.Emailing.Service;
using DeUrgenta.Infra.Extensions;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DeUrgenta.Domain.Api;
using Microsoft.EntityFrameworkCore;
using DeUrgenta.Domain.RecurringJobs;

namespace DeUrgenta.RecurringJobs
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabase<DeUrgentaContext>(Configuration.GetConnectionString("DbConnectionString"));
            services.AddDatabase<JobsContext>(Configuration.GetConnectionString("JobsConnectionString"));
            services.AddHangfireServices();

            services.AddRecurringJobs(Configuration);
            services.SetupEmailService(Configuration);

            services.SetupHealthChecks(Configuration);

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DeUrgentaContext dbContext)
        {
            dbContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseAuthenticatedHangfireDashboard(Configuration);
            app.ScheduleJobs(Configuration);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAppHealthChecks();

                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }
    }
}
