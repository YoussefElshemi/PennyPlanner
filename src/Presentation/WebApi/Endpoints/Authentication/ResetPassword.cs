using Core.Constants;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;
using FastEndpoints;
using FluentValidation;
using Presentation.WebApi.Models.Authentication;
using Presentation.WebApi.Validators;

namespace Presentation.WebApi.Endpoints.Authentication;

public class ResetPassword(IPasswordResetRepository passwordResetRepository,
    IUserService userService,
    TimeProvider timeProvider) : Endpoint<ResetPasswordRequestDto, ResetPasswordResponseDto>
{
    public override void Configure()
    {
        Post(ApiUrls.Authentication.ResetPassword);
        AllowAnonymous();
        EnableAntiforgery();
    }

    public override async Task HandleAsync(ResetPasswordRequestDto requestResetPasswordRequestDto, CancellationToken cancellationToken)
    {
        var validator = new ResetPasswordRequestDtoValidator(passwordResetRepository);
        await validator.ValidateAndThrowAsync(requestResetPasswordRequestDto, cancellationToken);

        var passwordReset = await passwordResetRepository.GetAsync(new PasswordResetToken(requestResetPasswordRequestDto.PasswordResetToken));
        passwordReset = passwordReset! with
        {
            IsUsed = new IsUsed(true),
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().DateTime)
        };
        await passwordResetRepository.UpdateAsync(passwordReset);

        var changePasswordRequest = new ChangePasswordRequest
        {
            Password = new Password(requestResetPasswordRequestDto.Password)
        };

        await userService.ChangePasswordAsync(passwordReset.User, changePasswordRequest);

        var response = new ResetPasswordResponseDto
        {
            Success = true
        };

        await SendAsync(response, cancellation: cancellationToken);
    }
}

public record ResetPasswordResponseDto
{
    public bool Success { get; init; }
}