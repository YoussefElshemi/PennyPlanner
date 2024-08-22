using Core.Constants;
using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;
using FastEndpoints;
using FluentValidation;
using Presentation.WebApi.Models.Authentication;
using Presentation.WebApi.Validators;

namespace Presentation.WebApi.Endpoints.Authentication;

public class RequestResetPassword(IAuthenticationService authenticationService) : Endpoint<RequestResetPasswordRequestDto, RequestResetPasswordResponseDto>
{
    public const string RequestResetPasswordMessage = "If an account exists with the given email address, you will receive an email to reset your password";

    public override void Configure()
    {
        Post(ApiUrls.Authentication.RequestResetPassword);
        AllowAnonymous();
        EnableAntiforgery();
    }

    public override async Task HandleAsync(RequestResetPasswordRequestDto requestResetPasswordRequestDto, CancellationToken cancellationToken)
    {
        var validator = new RequestResetPasswordRequestDtoValidator();
        await validator.ValidateAndThrowAsync(requestResetPasswordRequestDto, cancellationToken);

        var requestResetPasswordRequest = new RequestResetPasswordRequest
        {
            EmailAddress = new EmailAddress(requestResetPasswordRequestDto.EmailAddress)
        };

        await authenticationService.RequestResetPassword(requestResetPasswordRequest);

        var response = new RequestResetPasswordResponseDto
        {
            Message = RequestResetPasswordMessage
        };

        await SendAsync(response, cancellation: cancellationToken);
    }
}