using System.Security.Claims;
using System.Text.Json.Serialization;
using FastEndpoints;
using FastEndpoints.Swagger;
using Presentation.PreProcessors;

namespace Presentation.Extensions;

public static class WebApplicationExtensions
{
    public static void ConfigureMiddlewares(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgeryFE();
        app.UseCors();
        app.UseFastEndpoints(c =>
        {
            c.Endpoints.RoutePrefix = "api";
            c.Security.RoleClaimType = ClaimTypes.Role;
            c.Versioning.Prefix = "v";
            c.Versioning.DefaultVersion = 1;
            c.Versioning.PrependToRoute = true;
            c.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
            c.Endpoints.Configurator = e => { e.PreProcessor<AuthenticationPreProcessor>(Order.Before); };
        });

        app.UseExceptionHandler();
        app.UseSwaggerUi();
        app.UseSwaggerGen();
    }
}