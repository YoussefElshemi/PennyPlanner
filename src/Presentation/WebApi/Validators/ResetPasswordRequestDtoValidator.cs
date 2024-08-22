using Core.Extensions;
using Core.Interfaces.Repositories;
using Core.Validators;
using Core.ValueObjects;
using FluentValidation;
using Presentation.WebApi.Models.Authentication;

namespace Presentation.WebApi.Validators;

public class ResetPasswordRequestDtoValidator : AbstractValidator<ResetPasswordRequestDto>
{
    internal const string ConfirmPasswordErrorMessage = $"{nameof(Password)}s do not match.";
    internal const string PasswordResetTokenNotFoundErrorMessage = $"{nameof(PasswordResetToken)} not found.";
    internal const string PasswordResetTokenAlreadyUsedErrorMessage = $"{nameof(PasswordResetToken)} already used.";

    public ResetPasswordRequestDtoValidator()
    {
        RuleFor(x => x.PasswordResetToken)
            .NotEmpty();

        RuleFor(x => x.Password)
            .SetValidator(new PasswordValidator());

        RuleFor(x => new { x.Password, x.ConfirmPassword })
            .WithDisplayName(nameof(Password))
            .Must(x => x.Password == x.ConfirmPassword)
            .WithMessage(ConfirmPasswordErrorMessage);
    }

    public ResetPasswordRequestDtoValidator(IPasswordResetRepository passwordResetRepository)
    {
        RuleFor(x => x.PasswordResetToken)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (x, _) => await PasswordResetRequestExists(passwordResetRepository, x))
            .WithMessage(PasswordResetTokenNotFoundErrorMessage)
            .MustAsync(async (x, _) => await PasswordResetRequestNotUsed(passwordResetRepository, x))
            .WithMessage(PasswordResetTokenAlreadyUsedErrorMessage);

        RuleFor(x => x.Password)
            .SetValidator(new PasswordValidator());

        RuleFor(x => new { x.Password, x.ConfirmPassword })
            .Must(x => x.Password == x.ConfirmPassword)
            .WithMessage(ConfirmPasswordErrorMessage);
    }

    private static async Task<bool> PasswordResetRequestExists(IPasswordResetRepository passwordResetRepository, Guid passwordResetToken)
    {
        return await passwordResetRepository.ExistsAsync(new PasswordResetToken(passwordResetToken));
    }

    private static async Task<bool> PasswordResetRequestNotUsed(IPasswordResetRepository passwordResetRepository, Guid passwordResetToken)
    {
        var passwordReset = await passwordResetRepository.GetAsync(new PasswordResetToken(passwordResetToken));

        return passwordReset is not null && !passwordReset.IsUsed;
    }
}