using System.Linq;
using Amazon;
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
using Microsoft.Extensions.Hosting;

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
                //services.AddDefaultAWSOptions(configuration.GetAWSOptions());
                services.AddSingleton<IAmazonS3>(p =>
                {
                    var config = new AmazonS3Config
                    {
                        RegionEndpoint = RegionEndpoint.EnumerableAllRegions
                            .FirstOrDefault(r => r.DisplayName == configuration.GetValue<string>("AWS:Region"))
                    };
                    if (p.GetService<IHostEnvironment>().IsDevelopment())
                    {
                        //settings to use with localstack S3 service
                        config.ServiceURL = configuration.GetValue<string>("AWS:ServiceURL");
                        config.ForcePathStyle = true;
                    }

                    var awsAccessKeyId = configuration.GetValue<string>("AWS:AWS_ACCESS_KEY_ID");
                    var awsSecretAccessKey = configuration.GetValue<string>("AWS:AWS_SECRET_ACCESS_KEY");
                    return new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, config);
                });

                services.Configure<S3ConfigOptions>(configuration.GetSection(nameof(S3ConfigOptions)));
                services.AddSingleton<IBlobStorage, S3Storage>();
            }
        }
    }
}
