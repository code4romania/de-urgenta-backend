using DeUrgenta.Certifications.Api.Queries;
using DeUrgenta.Certifications.Api.Validators;
using DeUrgenta.Common.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace DeUrgenta.Certifications.Api
{
    public static class BootstrapingExtensions
    {
        public static IServiceCollection AddCertificationsApiServices(this IServiceCollection services)
        {
            services.AddTransient<IValidateRequest<GetCertifications>, GetCertificationsValidator>();
            return services;
        }
    }
}
