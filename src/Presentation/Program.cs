using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddExceptionHandling()
    .AddMappers()
    .AddFastEndpoints(builder.Configuration)
    .AddServices().AddAppConfiguration(builder.Configuration)
    .AddDataAccess(builder.Configuration)
    .AddAuthenticationSetup(builder.Configuration);

var app = builder.Build();

app.ConfigureMiddlewares();

app.Run();