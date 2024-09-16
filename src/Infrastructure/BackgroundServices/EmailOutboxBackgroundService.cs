using Core.Configs;
using Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace Infrastructure.BackgroundServices;

public class EmailOutboxBackgroundService(IServiceScopeFactory scopeFactory,
    IOptions<AppConfig> appConfig,
    ILogger logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested && appConfig.Value.BackgroundServiceConfig.EmailOutboxEnabled)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                await emailService.ProcessAwaitingEmailsAsync();
            }
            catch (Exception e)
            {
                logger.Error(e, "Error while processing email outbox: {error}", e.Message);
            }
            finally
            {
                await Task.Delay(appConfig.Value.BackgroundServiceConfig.EmailOutboxIntervalInMs, stoppingToken);
            }
        }
    }
}