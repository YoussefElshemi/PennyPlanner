using Core.Configs;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using FastEndpoints;
using FluentValidation;
using Microsoft.Extensions.Options;
using Presentation.Mappers;
using Presentation.WebApi.Models.Authentication;
using Presentation.WebApi.Validators;

namespace Presentation.WebApi.Endpoints.Authentication;

public class Register(IUserRepository userRepository,
    IAuthenticationService authenticationService,
    IOptions<AppConfig> config) : Endpoint<RegisterRequestDto, AuthenticationResponseDto>
    {
    public override void Configure()
    {
        Post("/auth/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterRequestDto registerRequestDto, CancellationToken cancellationToken)
    {
        var validator = new RegisterRequestDtoValidator(userRepository);
        await validator.ValidateAndThrowAsync(registerRequestDto, cancellationToken);

        var createUserRequest = CreateUserRequestMapper.Map(registerRequestDto);

        var authenticationResponse = await authenticationService.CreateUserAsync(createUserRequest);

        var response = AuthenticationResponseMapper.Map(authenticationResponse);

        await SendAsync(response, cancellation: cancellationToken);
    }
}