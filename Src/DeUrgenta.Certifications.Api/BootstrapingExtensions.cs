using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Certifications.Api.Models;
using DeUrgenta.Certifications.Api.Queries;
using DeUrgenta.Certifications.Api.Validators;
using DeUrgenta.Certifications.Api.Validators.RequestValidators;
using DeUrgenta.Common.Validation;
using FluentValidation;
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
            services.AddTransient<IValidator<CertificationRequest>, CertificationRequestValidator>();

            return services;
        }
    }
}
