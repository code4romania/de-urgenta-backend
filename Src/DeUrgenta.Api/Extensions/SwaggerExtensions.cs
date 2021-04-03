using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DeUrgenta.Infra.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Api.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerFor(this IServiceCollection services, Assembly[] assemblies, IConfiguration config)
        {
            var authorizeEndpoint = $"{config.GetValue<string>("IdentityServerUrl")}/connect/authorize";
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(authorizeEndpoint),
                            Scopes = new Dictionary<string, string>
                            {
                                { "usersApi", "Access read operations" },
                                { "backpackApi", "Access write operations" }
                            }
                        }
                    },
                    Description =
                       "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.OAuth2,
                    Scheme = "Bearer"
                });

                c.OperationFilter<AuthorizeCheckOperationFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "De Urgenta API",
                    Description = "The application aims to inform citizens about how to react to the first critical hours in a crysis situation (like that of an earthquake).",
                    TermsOfService = new Uri("https://github.com/code4romania/de-urgenta-backend"),
                    Contact = new OpenApiContact
                    {
                        Name = "Code4Romania",
                        Email = string.Empty,
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


            services.AddSwaggerExamplesFromAssemblies(assemblies);
            return services;
        }
    }
}
