using DeUrgenta.FirstAidCourse.Api.Commands;
using DeUrgenta.FirstAidCourse.Api.Models;
using DeUrgenta.FirstAidCourse.Api.Queries;
using DeUrgenta.FirstAidCourse.Api.Validators;
using DeUrgenta.FirstAidCourse.Api.Validators.RequestValidators;
using DeUrgenta.Common.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DeUrgenta.FirstAidCourse.Api
{
    public static class BootstrapingExtensions
    {
        public static IServiceCollection AddFirstAidCoursesApiServices(this IServiceCollection services)
        {
            services.AddTransient<IValidateRequest<GetFirstAidCourses>, GetFirstAidCoursesValidator>();
            services.AddTransient<IValidateRequest<CreateFirstAidCourse>, CreateFirstAidCourseValidator>();
            services.AddTransient<IValidateRequest<UpdateFirstAidCourse>, UpdateFirstAidCourseValidator>();
            services.AddTransient<IValidateRequest<DeleteFirstAidCourse>, DeleteFirstAidCourseValidator>();
            services.AddTransient<IValidator<FirstAidCourseRequest>, FirstAidCourseRequestValidator>();

            return services;
        }
    }
}
