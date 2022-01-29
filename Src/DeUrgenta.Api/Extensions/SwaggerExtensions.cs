using System;
using System.IO;
using System.Reflection;
using DeUrgenta.Api.Extensions.OperationFilter;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace DeUrgenta.Api.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerFor(this IServiceCollection services, Assembly[] assemblies)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme.ToLowerInvariant(),
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                };

                c.AddSecurityDefinition("Bearer", jwtSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
                c.CustomOperationIds(apiDesc => apiDesc.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name : $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.ActionDescriptor.RouteValues["action"]}_{apiDesc.HttpMethod}");

                c.OperationFilter<AuthorizeCheckOperationFilter>();
                c.OperationFilter<AcceptLanguageHeaderParameterOperationFilter>();

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "De Urgenta API",
                    Description = "The application aims to inform citizens about how to react to the first critical hours in a crisis situation (like that of an earthquake).",
                    TermsOfService = new Uri("https://github.com/code4romania/de-urgenta-backend"),
                    Contact = new OpenApiContact
                    {
                        Name = "Code4Romania",
                        Email = "code4ro@code4.ro",
                        Url = new Uri("https://code4.ro/"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under Mozilla Public License Version 2.0",
                        Url = new Uri("https://github.com/code4romania/de-urgenta-backend/blob/develop/LICENSE"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                foreach (var assembly in assemblies)
                {
                    var xmlFile = $"{assembly.GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                }

                c.ExampleFilters();
            });

            services.AddFluentValidationRulesToSwagger();
            services.AddSwaggerExamplesFromAssemblies(assemblies);

            return services;
        }

        public static IApplicationBuilder UseConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.ConfigObject = new ConfigObject
            {
                Urls = new[]
                    {
                        new UrlDescriptor{Name = "api", Url = "/swagger/v1/swagger.json"}
                    }
            });

            return app;
        }
    }
}
