using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using FastEndpoints;
using FluentValidation;
using Presentation.Mappers;
using Presentation.WebApi.Models.Authentication;
using Presentation.WebApi.Validators;

namespace Presentation.WebApi.Endpoints.Authentication;

public class Login(IUserRepository userRepository,
    IAuthenticationService authenticationService) : Endpoint<LoginRequestDto, AuthenticationResponseDto>
    {
    public override void Configure()
    {
        Post("/auth/login");
        AllowAnonymous();
        EnableAntiforgery();
    }

    public override async Task HandleAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken)
    {
        var validator = new LoginRequestDtoValidator(userRepository);
        await validator.ValidateAndThrowAsync(loginRequestDto, cancellationToken);

        var authenticationRequest = AuthenticationRequestMapper.Map(loginRequestDto);

        var authenticationResponse = await authenticationService.AuthenticateAsync(authenticationRequest);

        var response = AuthenticationResponseMapper.Map(authenticationResponse);

        await SendAsync(response, cancellation: cancellationToken);
    }
}