using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddExceptionHandling()
    .AddMappers()
    .AddServices()
    .AddFastEndpoints(builder.Configuration)
    .AddAppConfiguration(builder.Configuration)
    .AddDataAccess(builder.Configuration)
    .AddAuthenticationSetup(builder.Configuration);

var app = builder.Build();

app.ConfigureMiddlewares();

app.Run();

namespace Presentation
{
    public abstract partial class Program
    {
    }
}