﻿using DeUrgenta.Common.Validation;
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
            services.AddTransient<IValidateRequest<AcceptInvite>, AcceptInviteValidator>();
            services.AddTransient<IValidateRequest<AcceptGroupInvite>, AcceptGroupInviteValidator>();
            services.AddTransient<IValidateRequest<AcceptBackpackInvite>, AcceptBackpackInviteValidator>();

            services.AddScoped<InviteValidatorFactory>();

            services.AddScoped<CreateGroupInviteValidator>();
            services.AddScoped<CreateBackpackInviteValidator>();
            services.AddScoped<ICreateInviteValidator, CreateGroupInviteValidator>(s => s.GetService<CreateGroupInviteValidator>());
            services.AddScoped<ICreateInviteValidator, CreateBackpackInviteValidator>(s => s.GetService<CreateBackpackInviteValidator>());
            
            services.Configure<GroupsConfig>(configuration.GetSection(GroupsConfig.SectionName));
            services.Configure<BackpacksConfig>(configuration.GetSection(BackpacksConfig.SectionName));

            return services;
        }
    }
}
