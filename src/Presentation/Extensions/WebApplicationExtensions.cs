using System.Security.Claims;
using System.Text.Json.Serialization;
using Core.Configs;
using Core.Enums;
using FastEndpoints;
using FastEndpoints.Swagger;
using Presentation.PreProcessors;
using Serilog;

namespace Presentation.Extensions;

public static class WebApplicationExtensions
{
    public static void ConfigureMiddlewares(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgeryFE();
        app.UseCors();
        app.UseFastEndpoints(c =>
        {
            var appConfig = new AppConfig();
            app.Configuration.GetSection(nameof(AppConfig)).Bind(appConfig);

            c.Endpoints.RoutePrefix = "api";
            c.Security.RoleClaimType = ClaimTypes.Role;
            c.Versioning.Prefix = "v";
            c.Versioning.DefaultVersion = 1;
            c.Versioning.PrependToRoute = true;
            c.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
            c.Endpoints.Configurator = e =>
            {
                e.PreProcessor<AuthenticationPreProcessor>(Order.Before);

                if (appConfig.ServiceConfig.Environment == Core.Configs.Environment.Local)
                {
                    e.Roles(UserRole.User.ToString());
                }
            };
        });

        app.UseExceptionHandler();
        app.UseSwaggerUi();
        app.UseSwaggerGen();
    }
}