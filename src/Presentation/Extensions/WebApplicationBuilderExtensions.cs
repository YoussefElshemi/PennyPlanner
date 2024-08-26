using System.Reflection;
using Core.Configs;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Services;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using FluentValidation;
using Infrastructure;
using Infrastructure.Mappers;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Presentation.Constants;
using Presentation.ExceptionHandlers;
using Presentation.Mappers;
using Presentation.Mappers.Interfaces;

namespace Presentation.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfigurationBuilder configuration)
    {
        var config = configuration.AddEnvironmentVariables().Build();
        services.AddSingleton<IConfiguration>(config);
        services.AddOptions<AppConfig>().BindConfiguration(nameof(AppConfig));

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPasswordResetService, PasswordResetService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddSingleton<IPagedResponseMapper, PagedResponseMapper>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PennyPlannerDbContext>(opt => opt.UseSqlite(configuration["ConnectionStrings:SQLiteDefault"]));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILoginRepository, LoginRepository>();
        services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();

        return services;
    }

    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(PresentationProfile));
        services.AddAutoMapper(typeof(InfrastructureProfile));

        return services;
    }

    public static IServiceCollection AddFastEndpoints(this IServiceCollection services, IConfiguration configuration)
    {
        var appConfig = new AppConfig();
        configuration.GetSection(nameof(AppConfig)).Bind(appConfig);

        services.AddFastEndpoints();
        services.AddAntiforgery();
        services.AddCors();
        services.SwaggerDocument(o =>
        {
            o.ShortSchemaNames = true;
            o.RemoveEmptyRequestSchema = true;
            o.AutoTagPathSegmentIndex = 0;
            o.MaxEndpointVersion = 1;
            o.DocumentSettings = s =>
            {
                s.Title = appConfig.ServiceConfig.ServiceName;
                s.DocumentName = appConfig.ServiceConfig.ServiceName;
                s.Version = "v1";
            };

            o.TagDescriptions = t =>
            {
                t[SwaggerTags.Authentication] = SwaggerTagDescriptions.Authentication;
                t[SwaggerTags.User] = SwaggerTagDescriptions.User;
                t[SwaggerTags.UserManagement] = SwaggerTagDescriptions.UserManagement;
            };
        });

        return services;
    }

    public static void AddAuthenticationSetup(this IServiceCollection services, IConfiguration configuration)
    {
        var appConfig = new AppConfig();
        configuration.GetSection(nameof(AppConfig)).Bind(appConfig);

        services.AddAuthenticationJwtBearer(s => s.SigningKey = appConfig.JwtConfig.Key);
        services.AddAuthorization();
    }
}