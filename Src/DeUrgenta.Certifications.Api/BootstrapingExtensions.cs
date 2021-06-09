using DeUrgenta.Certifications.Api.Commands;
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
            services.AddTransient<IValidateRequest<CreateCertification>, CreateCertificationValidator>();
            services.AddTransient<IValidateRequest<UpdateCertification>, UpdateCertificationValidator>();
            services.AddTransient<IValidateRequest<DeleteCertification>, DeleteCertificationValidator>();

            return services;
        }
    }
}
