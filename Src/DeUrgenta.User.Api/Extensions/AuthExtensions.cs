using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using DeUrgenta.Infra.Extensions;
using DeUrgenta.User.Api.Domain;
using DeUrgenta.User.Api.Options;
using DeUrgenta.User.Api.Services;
using DeUrgenta.User.Api.Services.Emailing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DeUrgenta.User.Api.Extensions
{
    public static class AuthExtensions
    {
        private const string SecurityOptionsSectionName = "JwtConfig";

        public static IServiceCollection AddBearerAuth(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.ConfigureAppOptions<JwtConfig>(SecurityOptionsSectionName);

            var jwtConfig = services.GetOptions<JwtConfig>(SecurityOptionsSectionName);

            var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);

            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false,
                ClockSkew = TimeSpan.Zero
            };

            services.AddSingleton(tokenValidationParams);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = tokenValidationParams;
            });


            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;


            });

            services.AddTransient<IJwtService, JwtService>();

            services.AddDatabase<UserDbContext>(configuration.GetConnectionString("IdentityDbConnectionString"));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<UserDbContext>();


            return services;
        }

        public static void SetupEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEmailBuilderService, EmailBuilderService>();
            services.AddSingleton<ITemplateFileSelector, TemplateFileSelector>();


            var emailType = configuration.GetValue<EmailingSystemTypes>("EMailingSystem");

            var sp = services.BuildServiceProvider();
            var emailBuilder = sp.GetService<IEmailBuilderService>();

            switch (emailType)
            {
                case EmailingSystemTypes.SendGrid:
                    services.AddSingleton<IEmailSender, SendGridSender>(ctx =>
                        new SendGridSender(
                            emailBuilder,
                            new SendGridOptions
                            {
                                ApiKey = configuration["SendGrid:ApiKey"],
                                ClickTracking = configuration.GetValue<bool>("SendGrid:ClickTracking")
                            }
                        )
                    );
                    break;
                case EmailingSystemTypes.Smtp:
                    services.AddSingleton<IEmailSender, SmtpSender>(ctx =>
                        new SmtpSender(
                            emailBuilder,
                            new SmtpOptions
                            {
                                Host = configuration["Smtp:Host"],
                                Port = configuration.GetValue<int>("Smtp:Port"),
                                User = configuration["Smtp:User"],
                                Password = configuration["Smtp:Password"]
                            }
                        )
                    );
                    break;
            }
        }
    }
}
