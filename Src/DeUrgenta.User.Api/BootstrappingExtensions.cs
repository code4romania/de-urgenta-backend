using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using DeUrgenta.Common.Validation;
using DeUrgenta.Infra.Extensions;
using DeUrgenta.User.Api.Domain;
using DeUrgenta.User.Api.Options;
using DeUrgenta.User.Api.Queries;
using DeUrgenta.User.Api.Services;
using DeUrgenta.User.Api.Services.Emailing;
using DeUrgenta.User.Api.Validators;
using DeUrgenta.User.Api.Validators.RequestValidators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DeUrgenta.User.Api
{
    public static class BootstrappingExtensions
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

            services
                .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<UserDbContext>();

            services.AddTransient<IApplicationUserManager, ApplicationUserManager>();
            return services;
        }

        public static void SetupEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEmailBuilderService, EmailBuilderService>();
            services.AddSingleton<ITemplateFileSelector, TemplateFileSelector>();
            var emailType = configuration.GetValue<EmailingSystemTypes>("EMailingSystem");

            switch (emailType)
            {
                case EmailingSystemTypes.SendGrid:
                    services.AddSingleton(new SendGridOptions
                    {
                        ApiKey = configuration["SendGrid:ApiKey"],
                        ClickTracking = configuration.GetValue<bool>("SendGrid:ClickTracking")
                    });

                    services.AddSingleton<IEmailSender, SendGridSender>();
                    break;

                case EmailingSystemTypes.Smtp:
                    services.AddSingleton(new SmtpOptions
                    {
                        Host = configuration["Smtp:Host"],
                        Port = configuration.GetValue<int>("Smtp:Port"),
                        User = configuration["Smtp:User"],
                        Password = configuration["Smtp:Password"]
                    });

                    services.AddSingleton<IEmailSender, SmtpSender>();
                    break;
            }
        }

        public static IServiceCollection AddUserApiServices(this IServiceCollection services)
        {
            services.AddTransient<IValidateRequest<GetUser>, GetUserValidator>();
            services.AddTransient<IValidator<UserRequest>, UserRequestValidator>();
            services.AddTransient<IValidator<UserSafeLocationRequest>, UserSafeLocationRequestValidator>();

            services.AddTransient<IValidator<UserChangePasswordRequest>, UserChangePasswordRequestValidator>();
            services.AddTransient<IValidator<UserResetPasswordRequest>, UserResetPasswordRequestValidator>();
            services.AddTransient<IValidator<UserEmailPasswordResetRequest>, UserEmailPasswordResetRequestValidator>();
            return services;
        }
    }
}