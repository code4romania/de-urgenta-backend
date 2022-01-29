using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

namespace DeUrgenta.Infra.Extensions
{
    public static class HealthChecksExtensions
    {
        public static void MapAppHealthChecks(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecksUI();
            endpoints.MapHealthChecks("/health");
            endpoints.MapHealthChecks("/healthz", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }
    }
}