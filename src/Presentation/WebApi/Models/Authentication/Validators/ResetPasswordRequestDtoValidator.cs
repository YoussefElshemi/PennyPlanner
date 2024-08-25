using System.Net;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Validators;
using Core.ValueObjects;
using FluentValidation;

namespace Presentation.WebApi.Models.Authentication.Validators;

public class ResetPasswordRequestDtoValidator : AbstractValidator<ResetPasswordRequestDto>
{
    private readonly IPasswordResetRepository _passwordResetRepository;

    internal const string ConfirmPasswordErrorMessage = $"{nameof(Password)}s do not match.";
    internal const string PasswordResetTokenNotFoundErrorMessage = $"{nameof(PasswordResetToken)} not found.";
    internal const string PasswordResetTokenAlreadyUsedErrorMessage = $"{nameof(PasswordResetToken)} already used.";
    internal const string PasswordDidNotChangeErrorMessage = $"{nameof(Password)} did not change.";

    public ResetPasswordRequestDtoValidator(IAuthenticationService authenticationService,
        IPasswordResetRepository passwordResetRepository)
    {
        _passwordResetRepository = passwordResetRepository;

        RuleFor(x => x.PasswordResetToken)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (x, _) => await PasswordResetRequestExists(x))
            .WithErrorCode(HttpStatusCode.NotFound.ToString())
            .WithMessage(PasswordResetTokenNotFoundErrorMessage)
            .MustAsync(async (x, _) => await PasswordResetRequestNotUsed(x))
            .WithErrorCode(HttpStatusCode.Conflict.ToString())
            .WithMessage(PasswordResetTokenAlreadyUsedErrorMessage)
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
        return _passwordResetRepository.ExistsAsync(new PasswordResetToken(passwordResetToken));
    }

    private  async Task<bool> PasswordResetRequestNotUsed(string passwordResetToken)
    {
        var passwordReset = await _passwordResetRepository.GetAsync(new PasswordResetToken(passwordResetToken));

        return !passwordReset.IsUsed;
    }
}