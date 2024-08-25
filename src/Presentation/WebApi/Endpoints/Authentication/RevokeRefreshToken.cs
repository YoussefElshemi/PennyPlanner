using Core.Constants;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using FastEndpoints;
using FluentValidation;
using Presentation.Mappers;
using Presentation.WebApi.Models.Authentication;
using Presentation.WebApi.Validators.Authentication;

namespace Presentation.WebApi.Endpoints.Authentication;

public class RevokeRefreshToken(IAuthenticationService authenticationService,
    IValidator<RefreshTokenRequestDto> validator) : Endpoint<RefreshTokenRequestDto>
{
    public override void Configure()
    {
        Version(1);
        Post(ApiUrls.AuthenticationUrls.RevokeRefreshToken);
        AllowAnonymous();
        EnableAntiforgery();
    }

    public override async Task HandleAsync(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(refreshTokenRequestDto, cancellationToken);

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        var refreshTokenRequest = RefreshTokenRequestMapper.Map(refreshTokenRequestDto, ipAddress);

        await authenticationService.RevokeToken(refreshTokenRequest);

        await SendOkAsync(cancellation: cancellationToken);
    }
}