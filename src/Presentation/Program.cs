using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddFastEndpoints()
    .AddExceptionHandling()
    .AddServices().AddAppConfiguration(builder.Configuration)
    .AddDataAccess(builder.Configuration)
    .AddAuthenticationSetup(builder.Configuration);

var app = builder.Build();

app.ConfigureMiddlewares();

app.Run();
