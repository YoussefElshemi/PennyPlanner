using Core.Constants;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using FastEndpoints;
using FluentValidation;
using Presentation.Mappers;
using Presentation.WebApi.Models.Authentication;
using Presentation.WebApi.Validators;

namespace Presentation.WebApi.Endpoints.Authentication;

public class Register(IUserRepository userRepository,
    IAuthenticationService authenticationService) : Endpoint<RegisterRequestDto, AuthenticationResponseDto>
{
    public override void Configure()
    {
        Post(ApiUrls.AuthenticationUrls.Register);
        AllowAnonymous();
        EnableAntiforgery();
    }

    public override async Task HandleAsync(RegisterRequestDto registerRequestDto, CancellationToken cancellationToken)
    {
        var validator = new RegisterRequestDtoValidator(userRepository);
        await validator.ValidateAndThrowAsync(registerRequestDto, cancellationToken);

        var createUserRequest = CreateUserRequestMapper.Map(registerRequestDto);

        var authenticationResponse = await authenticationService.CreateAsync(createUserRequest);

        var response = AuthenticationResponseMapper.Map(authenticationResponse);

        await SendAsync(response, cancellation: cancellationToken);
    }
}