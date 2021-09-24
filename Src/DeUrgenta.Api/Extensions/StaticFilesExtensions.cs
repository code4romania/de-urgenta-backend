using System.IO;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace DeUrgenta.Api.Extensions
{
    public static class StaticFilesExtensions
    {
        public static void SetupStaticFiles(this IApplicationBuilder app, IConfiguration config, IWebHostEnvironment webHost)
        {
            var localCertificationStorePath = config.GetValue<string>("LocalConfigOptions:Path");
            var staticFilesRequestPath = config.GetValue<string>("LocalConfigOptions:StaticFilesRequestPath");
            
            if (!Directory.Exists(localCertificationStorePath))
            {
                Directory.CreateDirectory(localCertificationStorePath);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(webHost.ContentRootPath, localCertificationStorePath)),
                RequestPath = staticFilesRequestPath,
                ServeUnknownFileTypes = true,
                OnPrepareResponse = ctx =>
                {
                    var userIsAuthenticated = ctx.Context.User.Identity?.IsAuthenticated ?? false;
                    var userSub = ctx.Context.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                    var requestedPhotoBelongsToUser = userSub != null && (ctx.Context.Request.Path.Value?.StartsWith($"{staticFilesRequestPath}/{userSub}") ?? false);
                    
                    if (!userIsAuthenticated || !requestedPhotoBelongsToUser)
                    {
                        ctx.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                        ctx.Context.Response.ContentLength = 0;
                        ctx.Context.Response.Body = Stream.Null;
                        ctx.Context.Response.Headers.Add("Cache-Control", "no-store");
                    }
                }
            });
        }
    }
}
