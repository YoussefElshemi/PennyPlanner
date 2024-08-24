using Core.Constants;
using Core.Interfaces.Services;
using FastEndpoints;
using FluentValidation;
using Presentation.Mappers;
using Presentation.WebApi.Models.Authentication;
using Presentation.WebApi.Validators.Authentication;

namespace Presentation.WebApi.Endpoints.Authentication;

public class RequestResetPassword(IAuthenticationService authenticationService) : Endpoint<RequestResetPasswordRequestDto>
{
    public const string RequestResetPasswordMessage = "If an account exists with the given email address, you will receive an email to reset your password";

    public override void Configure()
    {
        Post(ApiUrls.AuthenticationUrls.RequestResetPassword);
        AllowAnonymous();
        EnableAntiforgery();
    }

    public override async Task HandleAsync(RequestResetPasswordRequestDto requestResetPasswordRequestDto, CancellationToken cancellationToken)
    {
        var validator = new RequestResetPasswordRequestDtoValidator();
        await validator.ValidateAndThrowAsync(requestResetPasswordRequestDto, cancellationToken);

        var requestResetPasswordRequest = RequestResetPasswordRequestMapper.Map(requestResetPasswordRequestDto);

        await authenticationService.RequestResetPassword(requestResetPasswordRequest);

        await SendNoContentAsync(cancellationToken);
    }
}