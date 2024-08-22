using Core.Validators;
using Core.ValueObjects;
using FluentValidation;
using Presentation.WebApi.Models.User;

namespace Presentation.WebApi.Validators;

public class ChangePasswordRequestDtoValidator : AbstractValidator<ChangePasswordRequestDto>
{
    internal const string ConfirmPasswordErrorMessage = $"{nameof(Password)}s do not match.";

    public ChangePasswordRequestDtoValidator()
    {
        RuleFor(x => x.Password)
            .SetValidator(new PasswordValidator());

        RuleFor(x => new { x.Password, x.ConfirmPassword })
            .Must(x => x.Password == x.ConfirmPassword)
            .WithMessage(ConfirmPasswordErrorMessage);
    }
}