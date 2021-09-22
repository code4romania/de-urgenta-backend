using Amazon.S3;
using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Certifications.Api.Models;
using DeUrgenta.Certifications.Api.Queries;
using DeUrgenta.Certifications.Api.Storage;
using DeUrgenta.Certifications.Api.Storage.Config;
using DeUrgenta.Certifications.Api.Validators;
using DeUrgenta.Certifications.Api.Validators.RequestValidators;
using DeUrgenta.Common.Validation;
using FluentValidation;
using Microsoft.Extensions.Configuration;
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

        public static void SetupStorageService(this IServiceCollection services, IConfiguration configuration)
        {
            var storageType = configuration.GetValue<StorageType>("StorageService");

            if (storageType == StorageType.Local)
            {
                services.Configure<LocalConfigOptions>(configuration.GetSection(nameof(LocalConfigOptions)));
                services.AddSingleton<IBlobStorage, LocalStorage>();
            }
            else
            {
                services.AddDefaultAWSOptions(configuration.GetAWSOptions());
                services.AddAWSService<IAmazonS3>();

                services.Configure<S3ConfigOptions>(configuration.GetSection(nameof(S3ConfigOptions)));
                services.AddSingleton<IBlobStorage, S3Storage>();
            }
        }
    }
}
