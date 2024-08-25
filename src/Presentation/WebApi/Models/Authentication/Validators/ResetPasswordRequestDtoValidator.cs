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
        IPasswordResetRepository passwordResetRepository,
        Core.Models.User user)
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
            .WithMessage(PasswordResetTokenAlreadyUsedErrorMessage);

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .SetValidator(new PasswordValidator())
            .Must(x => !authenticationService.Authenticate(user, new Password(x)))
            .WithErrorCode(HttpStatusCode.Conflict.ToString())
            .WithMessage(PasswordDidNotChangeErrorMessage);

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

    public ResetPasswordRequestDtoValidator(IPasswordResetRepository passwordResetRepository)
    {
        _passwordResetRepository = passwordResetRepository;
    }
}