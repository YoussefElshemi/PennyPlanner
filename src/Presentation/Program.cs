using System.Reflection;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Core.Configs;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using FastEndpoints.Security;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Presentation.ExceptionHandlers;
using Presentation.WebApi.PreProcessors;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFastEndpoints();
builder.Services.AddAntiforgery();
builder.Services.AddCors();
builder.Services.SwaggerDocument();

var configuration = builder.Configuration.AddEnvironmentVariables().Build();
builder.Services.AddSingleton<IConfiguration>(configuration);
builder.Services.AddOptions<AppConfig>().BindConfiguration(nameof(AppConfig));

var appConfig = new AppConfig();
builder.Configuration.GetSection(nameof(AppConfig)).Bind(appConfig);

builder.Services.AddDbContext<PennyPlannerDbContext>(opt => opt.UseSqlite(builder.Configuration["ConnectionStrings:SQLiteDefault"]));

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddAuthenticationJwtBearer(s => s.SigningKey = appConfig.JwtConfig.Key);

builder.Services.AddAuthorization();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgeryFE();
app.UseCors();
app.UseFastEndpoints(c =>
{
    c.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
    c.Security.RoleClaimType = ClaimTypes.Role;
    c.Endpoints.Configurator = e =>
    {
        e.PreProcessor<AuthenticationPreProcessor>(Order.Before);
    };
});
app.UseExceptionHandler();
app.UseSwaggerGen();
app.Run();
