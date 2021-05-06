using DeUrgenta.Common.Validation;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Queries;
using DeUrgenta.Group.Api.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace DeUrgenta.Group.Api
{
    public static class BootstrapingExtensions
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

            return services;
        }
    }
}