using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Backpack.Api.Validators;
using DeUrgenta.Backpack.Api.Validators.RequestValidators;
using DeUrgenta.Common.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DeUrgenta.Backpack.Api
{
    public static class BootstrappingExtensions
    {
        public static IServiceCollection AddBackpackApiServices(this IServiceCollection services)
        {
            services.AddTransient<IValidateRequest<CreateBackpack>, CreateBackpackValidator>();
            services.AddTransient<IValidateRequest<DeleteBackpack>, DeleteBackpackValidator>();
            services.AddTransient<IValidateRequest<GetBackpackContributors>, GetBackpackContributorsValidator>();
            services.AddTransient<IValidateRequest<GetBackpacks>, GetBackpacksValidator>();
            services.AddTransient<IValidateRequest<GetMyBackpacks>, GetMyBackpacksValidator>();
            services.AddTransient<IValidateRequest<RemoveContributor>, RemoveContributorValidator>();
            services.AddTransient<IValidateRequest<RemoveCurrentUserFromContributors>, RemoveCurrentUserFromContributorsValidator>();
            services.AddTransient<IValidateRequest<UpdateBackpack>, UpdateBackpackValidator>();
            services.AddTransient<IValidateRequest<AddBackpackItem>, AddBackpackItemValidator>();
            services.AddTransient<IValidateRequest<DeleteBackpackItem>, DeleteBackpackItemValidator>();
            services.AddTransient<IValidateRequest<UpdateBackpackItem>, UpdateBackpackItemValidator>();
            services.AddTransient<IValidateRequest<GetBackpackCategoryItems>, GetBackpackCategoryItemsValidator>();
            services.AddTransient<IValidateRequest<GetBackpackItems>, GetBackpackItemsValidator>();

            services.AddTransient<IValidator<BackpackItemRequest>, BackpackItemRequestValidator>();
            services.AddTransient<IValidator<BackpackModelRequest>, BackpackModelRequestValidator>();

            return services;
        }
    }
}