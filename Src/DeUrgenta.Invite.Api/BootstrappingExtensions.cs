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
            services.AddTransient<IValidateRequest<CreateInvite>, CreateInviteValidator>();

            services.AddScoped<InviteValidatorFactory>();

            services.AddScoped<CreateGroupInviteValidator>()
                .AddScoped<ICreateInviteValidator, CreateGroupInviteValidator>(s => s.GetService<CreateGroupInviteValidator>());
            services.AddScoped<CreateBackpackInviteValidator>()
                .AddScoped<ICreateInviteValidator, CreateBackpackInviteValidator>(s => s.GetService<CreateBackpackInviteValidator>());

            services.Configure<GroupsConfig>(configuration.GetSection(GroupsConfig.SectionName));

            return services;
        }
    }
}
