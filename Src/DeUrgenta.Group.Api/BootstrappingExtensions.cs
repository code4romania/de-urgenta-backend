using DeUrgenta.Common.Validation;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Queries;
using DeUrgenta.Group.Api.Validators;
using DeUrgenta.Group.Api.Validators.PayloadValidators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DeUrgenta.Group.Api
{
    public static class BootstrappingExtensions
    {
        public static IServiceCollection AddGroupApiServices(this IServiceCollection services)
        {
            services.AddTransient<IValidateRequest<AddGroup>, AddGroupValidator>();
            services.AddTransient<IValidateRequest<AddSafeLocation>, AddSafeLocationValidator>();
            services.AddTransient<IValidateRequest<DeleteGroup>, DeleteGroupValidator>();
            services.AddTransient<IValidateRequest<DeleteSafeLocation>, DeleteSafeLocationValidator>();
            services.AddTransient<IValidateRequest<GetAdministeredGroups>, GetAdministeredGroupsValidator>();
            services.AddTransient<IValidateRequest<GetGroupMembers>, GetGroupMembersValidator>();
            services.AddTransient<IValidateRequest<GetGroupSafeLocations>, GetGroupSafeLocationsValidator>();
            services.AddTransient<IValidateRequest<GetMyGroups>, GetMyGroupsValidator>();
            services.AddTransient<IValidateRequest<InviteToGroup>, InviteToGroupValidator>();
            services.AddTransient<IValidateRequest<LeaveGroup>, LeaveGroupValidator>();
            services.AddTransient<IValidateRequest<RemoveFromGroup>, RemoveFromGroupValidator>();
            services.AddTransient<IValidateRequest<UpdateGroup>, UpdateGroupValidator>();
            services.AddTransient<IValidateRequest<UpdateSafeLocation>, UpdateSafeLocationValidator>();
            services.AddTransient<IValidator<GroupRequest>, GroupPayloadValidator>();
            services.AddTransient<IValidator<SafeLocationRequest>, SafeLocationPayloadValidator>();

            return services;
        }
    }
}