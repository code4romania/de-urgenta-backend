using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Backpack.Api.Validators;
using DeUrgenta.Common.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace DeUrgenta.Backpack.Api
{
    public static class BootstrapingExtensions
    {
        public static IServiceCollection AddBackpackApiServices(this IServiceCollection services)
        {
            services.AddTransient<IValidateRequest<CreateBackpack>, CreateBackpackValidator>();
            services.AddTransient<IValidateRequest<DeleteBackpack>, DeleteBackpackValidator>();
            services.AddTransient<IValidateRequest<GetBackpackContributors>, GetBackpackContributorsValidator>();
            services.AddTransient<IValidateRequest<GetBackpacks>, GetBackpacksValidator>();
            services.AddTransient<IValidateRequest<GetMyBackpacks>, GetMyBackpacksValidator>();
            services.AddTransient<IValidateRequest<InviteToBackpackContributors>, InviteToBackpackContributorsValidator>();
            services.AddTransient<IValidateRequest<RemoveContributor>, RemoveContributorValidator>();
            services.AddTransient<IValidateRequest<RemoveCurrentUserFromContributors>, RemoveCurrentUserFromContributorsValidator>();
            services.AddTransient<IValidateRequest<UpdateBackpack>, UpdateBackpackValidator>();

            return services;
        }
    }
}