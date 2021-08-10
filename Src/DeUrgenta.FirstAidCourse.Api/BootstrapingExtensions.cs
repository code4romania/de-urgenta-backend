using Microsoft.Extensions.DependencyInjection;

namespace DeUrgenta.Courses.Api
{
    public static class BootstrapingExtensions
    {
        public static IServiceCollection AddCoursesApiServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
