using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Admin.Api.Queries;
using DeUrgenta.Admin.Api.Validators;
using DeUrgenta.Common.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace DeUrgenta.Admin.Api
{
    public static class BootstrappingExtensions
    {
        public static IServiceCollection AddAdminApiServices(this IServiceCollection services)
        {
            services.AddTransient<IValidateRequest<CreateEvent>, CreateEventValidator>();
            services.AddTransient<IValidateRequest<DeleteEvent>, DeleteEventValidator>();
            services.AddTransient<IValidateRequest<UpdateEvent>, UpdateEventValidator>();
            services.AddTransient<IValidateRequest<GetEvents>, GetEventsValidator>();

            return services;
        }
    }
}
