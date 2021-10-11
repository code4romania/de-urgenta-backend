using DeUrgenta.Common.Validation;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Models;
using DeUrgenta.Invite.Api.Options;
using DeUrgenta.Invite.Api.Validators;
using DeUrgenta.Invite.Api.Validators.RequestValidators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeUrgenta.Invite.Api
{
    public static class BootstrappingExtensions
    {
        public static IServiceCollection AddInviteApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IValidator<InviteRequest>, InviteRequestValidator>();
            services.AddTransient<IValidator<AcceptInviteRequest>, AcceptInviteRequestValidator>();
            services.AddTransient<IValidateRequest<CreateInvite>, CreateInviteValidator>();
            services.AddTransient<IValidateRequest<AcceptInvite>, AcceptInviteValidator>();

            services.AddScoped<InviteValidatorFactory>();

            services.AddScoped<CreateGroupInviteValidator>();
            services.AddScoped<CreateBackpackInviteValidator>();
            services.AddScoped<ICreateInviteValidator, CreateGroupInviteValidator>(s => s.GetService<CreateGroupInviteValidator>());
            services.AddScoped<ICreateInviteValidator, CreateBackpackInviteValidator>(s => s.GetService<CreateBackpackInviteValidator>());

            services.AddScoped<AcceptGroupInviteValidator>();
            services.AddScoped<AcceptBackpackInviteValidator>();
            services.AddScoped<IAcceptInviteValidator, AcceptGroupInviteValidator>(s => s.GetService<AcceptGroupInviteValidator>());
            services.AddScoped<IAcceptInviteValidator, AcceptBackpackInviteValidator>(s => s.GetService<AcceptBackpackInviteValidator>());


            services.Configure<GroupsConfig>(configuration.GetSection(GroupsConfig.SectionName));

            return services;
        }
    }
}
