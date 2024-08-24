using Core.Constants;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using FastEndpoints;
using FluentValidation;
using Presentation.Mappers;
using Presentation.WebApi.Models.Authentication;
using Presentation.WebApi.Validators;

namespace Presentation.WebApi.Endpoints.Authentication;

public class RevokeRefreshToken(ILoginRepository loginRepository,
    IAuthenticationService authenticationService,
    TimeProvider timeProvider) : Endpoint<RefreshTokenRequestDto>
{
    public override void Configure()
    {
        Post(ApiUrls.AuthenticationUrls.RevokeRefreshToken);
        AllowAnonymous();
        EnableAntiforgery();
    }

    public override async Task HandleAsync(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken)
    {
        var validator = new RefreshTokenRequestDtoValidator(loginRepository, timeProvider);
        await validator.ValidateAndThrowAsync(refreshTokenRequestDto, cancellationToken);

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        var refreshTokenRequest = RefreshTokenRequestMapper.Map(refreshTokenRequestDto, ipAddress);

        await authenticationService.RevokeToken(refreshTokenRequest);

        await SendOkAsync(cancellation: cancellationToken);
    }
}