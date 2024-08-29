using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Events;

public class RequestPasswordResetEvent(IServiceScopeFactory scopeFactory) : IEventHandler<RequestPasswordResetRequest>
{
    public async Task HandleAsync(RequestPasswordResetRequest requestPasswordResetRequest, CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var authenticationService = scope.Resolve<IAuthenticationService>();

        await authenticationService.RequestPasswordReset(requestPasswordResetRequest);
    }
}