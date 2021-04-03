using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DeUrgenta.IdentityServer;
using DeUrgenta.IdentityServer.Helpers;
using DeUrgenta.IdentityServer.Options;
using DeUrgenta.IdentityServer.Services.Emailing;
using IdentityServer.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;
            _identityConfiguration = new DeUrgentaIdentityConfiguration(configuration);
        }

        public IConfiguration Configuration { get; }
        private readonly IDeUrgentaIdentityConfiguration _identityConfiguration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            var allowedRedirectUrls = new List<string>();
            Configuration.GetSection("AllowedRedirectUrls").Bind(allowedRedirectUrls);

            var allowedRedirects = new AllowedRedirects
            {
                Urls = allowedRedirectUrls
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(UrlHelpers.NormalizeUrl)
                    .Where(url => _env.IsDevelopment() || url.StartsWith("https")) // on prod do not allow redirects to http
                    .Distinct()
                    .ToList()
            };

            services.AddSingleton(allowedRedirects);
            services.AddRazorPages();
            services.AddControllersWithViews();

            var builder = services.AddIdentityServer(options =>
                {
                    var publicOrigin = Configuration["IdentityServerPublicOrigin"];
                    if (!string.IsNullOrEmpty(publicOrigin))
                    {
                        options.PublicOrigin = publicOrigin;
                    }

                    options.UserInteraction.LoginUrl = "/account/login";
                    options.UserInteraction.LogoutUrl = "/account/logout";

                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddInMemoryIdentityResources(_identityConfiguration.Ids)
                .AddInMemoryApiResources(_identityConfiguration.Apis())
                .AddInMemoryClients(_identityConfiguration.Clients)
                .AddAspNetIdentity<ApplicationUser>();
            services.AddAuthentication();
            var base64EncodedCertificate = Configuration["Certificate:Base64Encoded"];
            var password = Configuration["Certificate:Password"];

            builder.AddSigningCredential(LoadCertificate(base64EncodedCertificate, password));

            services.AddTransient<IEmailBuilderService, EmailBuilderService>();
            services.AddSingleton<ITemplateFileSelector, TemplateFileSelector>();

            
            var emailType = Configuration.GetValue<EmailingSystemTypes>("EMailingSystem");

            var sp = services.BuildServiceProvider();
            var emailBuilder = sp.GetService<IEmailBuilderService>();

            switch (emailType)
            {
                case EmailingSystemTypes.SendGrid:
                    services.AddSingleton<IEmailSender>(ctx =>
                        new SendGridSender(
                            emailBuilder,
                            new SendGridOptions
                            {
                                ApiKey = Configuration["SendGrid:ApiKey"],
                                ClickTracking = Configuration.GetValue<bool>("SendGrid:ClickTracking")
                            }
                        )
                    );
                    break;
                case EmailingSystemTypes.Smtp:
                    services.AddSingleton<IEmailSender>(ctx =>
                        new SmtpSender(
                            emailBuilder,
                            new SmtpOptions
                            {
                                Host = Configuration["Smtp:Host"],
                                Port = Configuration.GetValue<int>("Smtp:Port"),
                                User = Configuration["Smtp:User"],
                                Password = Configuration["Smtp:Password"]
                            }
                        )
                    );
                    break;
            }

            services.AddSingleton<PasswordValidationMessages>();
            if (!_env.IsDevelopment())
                services.ConfigureApplicationCookie(options =>
                {
                    options.Cookie.SameSite = SameSiteMode.None;
                });
            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy = _identityConfiguration.Clients.SelectMany(x => x.AllowedCorsOrigins)
                        .Aggregate(policy, (current, url) => current.WithOrigins(url));
                    policy.AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        private X509Certificate2 LoadCertificate(string base64EncodedCertificate, string password)
        {
            var certificateBytes = Convert.FromBase64String(base64EncodedCertificate);

            var certificate = new X509Certificate2(certificateBytes, password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            return certificate;
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHttpsRedirection();
            }

            app.UseCors("default");
            app.UseRouting();
            app.UseStaticFiles();
            var cookiePolicyOptions = new CookiePolicyOptions
            {
                // Mark cookies as `Secure` (only if using HTTPS in development, and always in production)
                Secure = env.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always,
            };
            app.UseCookiePolicy(cookiePolicyOptions);

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
