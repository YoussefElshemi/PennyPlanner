using Core.Constants;
using Core.Interfaces.Services;
using FastEndpoints;
using FluentValidation;
using Presentation.Mappers;
using Presentation.WebApi.Models.Authentication;

namespace Presentation.WebApi.Endpoints.Authentication;

public class RequestResetPassword(IAuthenticationService authenticationService,
    IValidator<RequestResetPasswordRequestDto> validator) : Endpoint<RequestResetPasswordRequestDto>
{
    public override void Configure()
    {
        Version(1);
        Post(ApiUrls.AuthenticationUrls.RequestResetPassword);
        AllowAnonymous();
        EnableAntiforgery();
    }

    public override async Task HandleAsync(RequestResetPasswordRequestDto requestResetPasswordRequestDto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(requestResetPasswordRequestDto, cancellationToken);

        var requestResetPasswordRequest = RequestResetPasswordRequestMapper.Map(requestResetPasswordRequestDto);

        await authenticationService.RequestResetPassword(requestResetPasswordRequest);

        await SendNoContentAsync(cancellationToken);
    }
}