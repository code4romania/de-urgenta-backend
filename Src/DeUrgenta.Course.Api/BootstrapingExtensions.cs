using DeUrgenta.Courses.Api.Validators;
using DeUrgenta.Courses.Api.Queries;
using Microsoft.Extensions.DependencyInjection;
using DeUrgenta.Common.Validation;

namespace DeUrgenta.Courses.Api
{
    public static class BootstrapingExtensions
    {
        public static IServiceCollection AddCoursesApiServices(this IServiceCollection services)
        {
            services.AddTransient<IValidateRequest<GetCourseTypes>, GetCourseTypesValidator>();
            services.AddTransient<IValidateRequest<GetCourseCities>, GetCourseCitiesValidator>();
            services.AddTransient<IValidateRequest<GetEvents>, GetEventsValidator>();

            return services;
        }
    }
}
