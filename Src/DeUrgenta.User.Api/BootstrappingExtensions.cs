using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using DeUrgenta.Common.Validation;
using DeUrgenta.Infra.Extensions;
using DeUrgenta.User.Api.Commands;
using DeUrgenta.User.Api.Domain;
using DeUrgenta.User.Api.Models;
using DeUrgenta.User.Api.Models.DTOs.Requests;
using DeUrgenta.User.Api.Options;
using DeUrgenta.User.Api.Queries;
using DeUrgenta.User.Api.Services;
using DeUrgenta.User.Api.Validators;
using DeUrgenta.User.Api.Validators.RequestValidators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DeUrgenta.User.Api
{
    public static class BootstrappingExtensions
    {
        private const string SecurityOptionsSectionName = "JwtConfig";
        private const string PasswordOptionsSectionName = "Passwords";

        public static IServiceCollection AddBearerAuth(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.ConfigureAppOptions<JwtConfig>(SecurityOptionsSectionName);
            services.ConfigureAppOptions<PasswordOptions>(PasswordOptionsSectionName);

            var jwtConfig = services.GetOptions<JwtConfig>(SecurityOptionsSectionName);
            var passwordOptions = services.GetOptions<PasswordOptions>(PasswordOptionsSectionName);

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

            services.Configure<IdentityOptions>(options => options.Password = passwordOptions);

            services.AddTransient<IJwtService, JwtService>();

            services.AddDatabase<UserDbContext>(configuration.GetConnectionString("IdentityDbConnectionString"));

            services
                .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<UserDbContext>();

            services.AddTransient<IApplicationUserManager, ApplicationUserManager>();
            return services;
        }

        public static IApplicationBuilder ConfigureBearerAuth(this IApplicationBuilder builder)
        {
            builder.ApplicationServices.UseDatabase<UserDbContext>();
            return builder;
        }
        
        public static IServiceCollection AddUserApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GroupsConfig>(configuration.GetSection(GroupsConfig.SectionName));
            
            services.AddTransient<IValidateRequest<GetUser>, GetUserValidator>();
            services.AddTransient<IValidateRequest<UpdateUser>, UpdateUserValidator>();
            services.AddTransient<IValidateRequest<GetBackpackInvites>, GetBackpackInvitesValidator>();
            services.AddTransient<IValidateRequest<GetGroupInvites>, GetGroupInvitesValidator>();
            services.AddTransient<IValidateRequest<GetUserLocations>, GetUserLocationsValidator>();

            services.AddTransient<IValidateRequest<AcceptBackpackInvite>, AcceptBackpackInviteValidator>();
            services.AddTransient<IValidateRequest<AcceptGroupInvite>, AcceptGroupInviteValidator>();
            services.AddTransient<IValidateRequest<RejectBackpackInvite>, RejectBackpackInviteValidator>();
            services.AddTransient<IValidateRequest<RejectGroupInvite>, RejectGroupInviteValidator>();

            services.AddTransient<IValidateRequest<AddLocation>, AddLocationValidator>();
            services.AddTransient<IValidateRequest<DeleteLocation>, DeleteLocationValidator>();
            services.AddTransient<IValidateRequest<UpdateLocation>, UpdateLocationValidator>();

            services.AddTransient<IValidator<UserRequest>, UserRequestValidator>();
            services.AddTransient<IValidator<UserLocationRequest>, UserSafeLocationRequestValidator>();

            services.AddTransient<IValidator<UserChangePasswordRequest>, UserChangePasswordRequestValidator>();
            services.AddTransient<IValidator<UserResetPasswordRequest>, UserResetPasswordRequestValidator>();
            services.AddTransient<IValidator<UserEmailPasswordResetRequest>, UserEmailPasswordResetRequestValidator>();
            return services;
        }
    }
}