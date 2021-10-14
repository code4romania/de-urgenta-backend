using System.Reflection;
using DeUrgenta.Admin.Api.Controller;
using DeUrgenta.Backpack.Api.Controllers;
using DeUrgenta.Certifications.Api.Controller;
using DeUrgenta.Api.Extensions;
using DeUrgenta.Backpack.Api;
using DeUrgenta.Common.Swagger;
using DeUrgenta.Group.Api;
using DeUrgenta.Group.Api.Controllers;
using Hellang.Middleware.ProblemDetails;
using DeUrgenta.Infra.Extensions;
using DeUrgenta.User.Api;
using DeUrgenta.User.Api.Controller;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DeUrgenta.Certifications.Api;
using FluentValidation.AspNetCore;
using DeUrgenta.Admin.Api;
using DeUrgenta.Domain.Api;
using DeUrgenta.Emailing.Service;
using DeUrgenta.Events.Api;
using DeUrgenta.Events.Api.Controller;
using DeUrgenta.Invite.Api;
using DeUrgenta.Invite.Api.Controllers;

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
        private const string CorsPolicyName = "MyPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBearerAuth(Configuration);
            services.AddControllers().AddFluentValidation();
            services.AddDatabase<DeUrgentaContext>(Configuration.GetConnectionString("DbConnectionString"));
            services.AddExceptionHandling(WebHostEnvironment);

            services.AddUserApiServices(Configuration);
            services.AddBackpackApiServices();
            services.AddGroupApiServices(Configuration);
            services.AddCertificationsApiServices();
            services.AddEventsApiServices();
            services.AddAdminApiServices();
            services.AddInviteApiServices(Configuration);

            var applicationAssemblies = GetAssemblies();

            services.AddSwaggerFor(applicationAssemblies, Configuration);
            services.AddMediatR(applicationAssemblies);

            services.AddCors(o => o.AddPolicy(CorsPolicyName, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.SetupEmailService(Configuration);
            services.SetupStorageService(Configuration);

            services.SetupHealthChecks(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (WebHostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseProblemDetails();
            app.UseConfigureSwagger();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapAppHealthChecks();

                endpoints.MapControllers();
            });

            app.SetupStaticFiles(Configuration, WebHostEnvironment);
           
            app.UseCors(CorsPolicyName);
        }

        private static Assembly[] GetAssemblies() => new[]
            {
                Assembly.GetAssembly(typeof(Startup)),

                // Application parts
                typeof(BackpackController).GetTypeInfo().Assembly,
                typeof(CertificationController).GetTypeInfo().Assembly,
                typeof(AdminBlogController).GetTypeInfo().Assembly,
                typeof(GroupController).GetTypeInfo().Assembly,
                typeof(UserController).GetTypeInfo().Assembly,
                typeof(EventController).GetTypeInfo().Assembly,
                typeof(InviteController).GetTypeInfo().Assembly,

                // Common

                typeof(ApplicationErrorResponseExample).GetTypeInfo().Assembly
            };
    }
}
