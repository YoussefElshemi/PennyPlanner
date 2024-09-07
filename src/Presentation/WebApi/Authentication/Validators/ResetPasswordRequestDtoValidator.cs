using System.Net;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Validators;
using Core.ValueObjects;
using FluentValidation;
using Presentation.WebApi.Authentication.Models.Requests;

namespace Presentation.WebApi.Authentication.Validators;

public class ResetPasswordRequestDtoValidator : AbstractValidator<ResetPasswordRequestDto>
{
    internal const string ConfirmPasswordErrorMessage = $"{nameof(Password)}s do not match.";
    internal const string PasswordResetTokenNotFoundErrorMessage = $"{nameof(PasswordResetToken)} not found.";
    internal const string PasswordResetTokenAlreadyUsedErrorMessage = $"{nameof(PasswordResetToken)} already used.";
    internal const string PasswordResetTokenExpiredErrorMessage = $"{nameof(PasswordResetToken)} has expired.";
    internal const string PasswordDidNotChangeErrorMessage = $"{nameof(Password)} is the same as the current value.";

    private readonly IPasswordResetRepository _passwordResetRepository;
    private readonly TimeProvider _timeProvider;

    public ResetPasswordRequestDtoValidator(IAuthenticationService authenticationService,
        IPasswordResetRepository passwordResetRepository, TimeProvider timeProvider)
    {
        _passwordResetRepository = passwordResetRepository;
        _timeProvider = timeProvider;

        RuleFor(x => x.PasswordResetToken)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (x, _) => await PasswordResetRequestExists(x))
            .WithErrorCode(HttpStatusCode.NotFound.ToString())
            .WithMessage(PasswordResetTokenNotFoundErrorMessage)
            .MustAsync(async (x, _) => await PasswordResetRequestNotUsed(x))
            .WithErrorCode(HttpStatusCode.Conflict.ToString())
            .WithMessage(PasswordResetTokenAlreadyUsedErrorMessage)
            .MustAsync(async (x, _) => await PasswordResetRequestNotExpired(x))
            .WithErrorCode(HttpStatusCode.Gone.ToString())
            .WithMessage(PasswordResetTokenExpiredErrorMessage)
            .DependentRules(() =>
            {
                RuleFor(x => new { x.Password, x.PasswordResetToken })
                    .MustAsync(async (x, _) => !authenticationService.Authenticate(
                        (await passwordResetRepository.GetAsync(x.PasswordResetToken)).User,
                        new Password(x.Password)))
                    .WithErrorCode(HttpStatusCode.Conflict.ToString())
                    .WithMessage(PasswordDidNotChangeErrorMessage);
            });

        RuleFor(x => x.Password)
            .SetValidator(new PasswordValidator());

        RuleFor(x => new { x.Password, x.ConfirmPassword })
            .Must(x => x.Password == x.ConfirmPassword)
            .WithMessage(ConfirmPasswordErrorMessage);
    }

    private Task<bool> PasswordResetRequestExists(string passwordResetToken)
    {
        return _passwordResetRepository.ExistsAsync(passwordResetToken);
    }

    private async Task<bool> PasswordResetRequestNotUsed(string passwordResetToken)
    {
        var passwordReset = await _passwordResetRepository.GetAsync(passwordResetToken);

        return !passwordReset.IsUsed;
    }

    private async Task<bool> PasswordResetRequestNotExpired(string passwordResetToken)
    {
        var passwordReset = await _passwordResetRepository.GetAsync(passwordResetToken);

        return passwordReset.ExpiresAt >= _timeProvider.GetUtcNow().UtcDateTime;
    }
}