using Core.Configs;
using Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Infrastructure.BackgroundServices;

public class EmailOutboxBackgroundService(IServiceScopeFactory scopeFactory,
    IOptions<AppConfig> appConfig) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            await emailService.ProcessAwaitingEmailsAsync();

            await Task.Delay(appConfig.Value.BackgroundServiceConfig.EmailOutboxIntervalInMs, stoppingToken);
        }
    }
}