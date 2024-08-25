using Core.Constants;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using FastEndpoints;
using FluentValidation;
using Presentation.Mappers;
using Presentation.WebApi.Models.Authentication;
using Presentation.WebApi.Validators.Authentication;

namespace Presentation.WebApi.Endpoints.Authentication;

public class Register(IUserRepository userRepository,
    IAuthenticationService authenticationService,
    IValidator<RegisterRequestDto> validator) : Endpoint<RegisterRequestDto, AuthenticationResponseDto>
{
    public override void Configure()
    {
        Post(ApiUrls.AuthenticationUrls.Register);
        AllowAnonymous();
        EnableAntiforgery();
    }

    public override async Task HandleAsync(RegisterRequestDto registerRequestDto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(registerRequestDto, cancellationToken);

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        var createUserRequest = CreateUserRequestMapper.Map(registerRequestDto, ipAddress);

        var authenticationResponse = await authenticationService.CreateAsync(createUserRequest);

        var response = AuthenticationResponseMapper.Map(authenticationResponse);

        await SendAsync(response, cancellation: cancellationToken);
    }
}