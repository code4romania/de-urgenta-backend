using Microsoft.Extensions.DependencyInjection;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Api.Extensions
{
    internal static class DbExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DeUrgentaContext>(o => o.UseSqlServer(connectionString));

            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DeUrgentaContext>();
            dbContext.Database.Migrate();

            return services;
        }
    }
}
