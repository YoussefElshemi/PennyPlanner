using System.Reflection;
using Core.Configs;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Presentation.ExceptionHandlers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PennyPlannerDbContext>(opt => opt.UseInMemoryDatabase("PennyPlannerDbContext"));
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

var configuration = builder.Configuration.AddEnvironmentVariables().Build();
builder.Services.AddSingleton<IConfiguration>(configuration);
builder.Services.AddOptions<AppConfig>().BindConfiguration(nameof(AppConfig));

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

var app = builder.Build();
app.UseFastEndpoints();
app.UseSwaggerGen();
app.UseExceptionHandler();
app.Run();