using Core.Constants;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using FastEndpoints;
using FluentValidation;
using Presentation.Mappers;
using Presentation.WebApi.Models.Authentication;
using Presentation.WebApi.Validators.Authentication;

namespace Presentation.WebApi.Endpoints.Authentication;

public class Login(IAuthenticationService authenticationService,
    IValidator<LoginRequestDto> validator) : Endpoint<LoginRequestDto, AuthenticationResponseDto>
{
    public override void Configure()
    {
        Version(1);
        Post(ApiUrls.AuthenticationUrls.Login);
        AllowAnonymous();
        EnableAntiforgery();
    }

    public override async Task HandleAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(loginRequestDto, cancellationToken);

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        var authenticationRequest = AuthenticationRequestMapper.Map(loginRequestDto, ipAddress);

        var authenticationResponse = await authenticationService.AuthenticateAsync(authenticationRequest);

        var response = AuthenticationResponseMapper.Map(authenticationResponse);

        await SendAsync(response, cancellation: cancellationToken);
    }
}