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
            services.AddTransient<IValidateRequest<AddBackpackItem>, AddBackpackItemValidator>();
            services.AddTransient<IValidateRequest<DeleteBackpackItem>, DeleteBackpackItemValidator>();
            services.AddTransient<IValidateRequest<UpdateBackpackItem>, UpdateBackpackItemValidator>();
            services.AddTransient<IValidateRequest<GetBackpackCategoryItems>, GetBackpackCategoryItemsValidator>();
            services.AddTransient<IValidateRequest<GetBackpackItems>, GetBackpackItemsValidator>();

            return services;
        }
    }
}
