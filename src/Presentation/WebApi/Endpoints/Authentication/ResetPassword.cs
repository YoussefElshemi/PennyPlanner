using Core.Constants;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using FastEndpoints;
using FluentValidation;
using Presentation.Mappers;
using Presentation.WebApi.Models.Authentication;
using Presentation.WebApi.Validators;

namespace Presentation.WebApi.Endpoints.Authentication;

public class ResetPassword(IPasswordResetRepository passwordResetRepository,
    IAuthenticationService authenticationService) : Endpoint<ResetPasswordRequestDto>
{
    public override void Configure()
    {
        Post(ApiUrls.AuthenticationUrls.ResetPassword);
        AllowAnonymous();
        EnableAntiforgery();
    }

    public override async Task HandleAsync(ResetPasswordRequestDto requestResetPasswordRequestDto, CancellationToken cancellationToken)
    {
        var validator = new ResetPasswordRequestDtoValidator(passwordResetRepository);
        await validator.ValidateAndThrowAsync(requestResetPasswordRequestDto, cancellationToken);

        var resetPasswordReset = ResetPasswordRequestMapper.Map(requestResetPasswordRequestDto);

        await authenticationService.ResetPassword(resetPasswordReset);

        await SendNoContentAsync(cancellationToken);
    }
}