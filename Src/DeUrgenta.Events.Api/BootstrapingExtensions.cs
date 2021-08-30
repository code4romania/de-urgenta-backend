using DeUrgenta.Events.Api.Validators;
using DeUrgenta.Events.Api.Queries;
using Microsoft.Extensions.DependencyInjection;
using DeUrgenta.Common.Validation;

namespace DeUrgenta.Events.Api
{
    public static class BootstrapingExtensions
    {
        public static IServiceCollection AddEventsApiServices(this IServiceCollection services)
        {
            services.AddTransient<IValidateRequest<GetEventTypes>, GetEventTypesValidator>();
            services.AddTransient<IValidateRequest<GetEventCities>, GetEventCitiesValidator>();
            services.AddTransient<IValidateRequest<GetEvents>, GetEventsValidator>();

            return services;
        }
    }
}
