using System.Security.Claims;
using Core.Interfaces.Repositories;
using Core.Models;
using FastEndpoints;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.WebApi.PreProcessors;

public class AuthenticationPreProcessor(IServiceScopeFactory scopeFactory) : IGlobalPreProcessor
{
    private const string UserDoesNotExistErrorMessage = "User does not exist.";

    public async Task PreProcessAsync(IPreProcessorContext ctx, CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var userRepository = scope.Resolve<IUserRepository>();

        if (ctx.HttpContext.User.Identity is { IsAuthenticated: true })
        {
            var requiresAuth = ctx.HttpContext.GetEndpoint()?.Metadata.OfType<AllowAnonymousAttribute>().Any() is not (null or true);
            if (requiresAuth)
            {
                var userId = ctx.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
                if (!await userRepository.ExistsByIdAsync(Guid.Parse(userId)))
                {
                    throw new ValidationException([new ValidationFailure(nameof(User),UserDoesNotExistErrorMessage)]);
                }

                var user = await userRepository.GetByIdAsync(Guid.Parse(userId));
                ctx.HttpContext.Items[nameof(User)] = user;
            }
        }
    }
}