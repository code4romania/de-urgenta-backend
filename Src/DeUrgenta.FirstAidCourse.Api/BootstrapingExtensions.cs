using DeUrgenta.Courses.Api.Commands;
using DeUrgenta.Courses.Api.Models;
using DeUrgenta.Courses.Api.Queries;
using DeUrgenta.Courses.Api.Validators;
using DeUrgenta.Courses.Api.Validators.RequestValidators;
using DeUrgenta.Common.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DeUrgenta.Courses.Api
{
    public static class BootstrapingExtensions
    {
        public static IServiceCollection AddFirstAidCoursesApiServices(this IServiceCollection services)
        {
            services.AddTransient<IValidateRequest<GetCourses>, GetFirstAidCoursesValidator>();
            services.AddTransient<IValidateRequest<CreateCourse>, CreateFirstAidCourseValidator>();
            services.AddTransient<IValidateRequest<UpdateCourse>, UpdateFirstAidCourseValidator>();
            services.AddTransient<IValidateRequest<DeleteCourse>, DeleteFirstAidCourseValidator>();
            services.AddTransient<IValidator<CourseRequest>, FirstAidCourseRequestValidator>();

            return services;
        }
    }
}
