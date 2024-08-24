using Core.Extensions;
using Core.Models;
using Core.Validators;
using Core.ValueObjects;
using FluentValidation;
using Presentation.WebApi.Models.User;

namespace Presentation.WebApi.Validators;

public class ChangePasswordRequestDtoValidator : AbstractValidator<ChangePasswordRequestDto>
{
    internal const string ConfirmPasswordErrorMessage = $"{nameof(Password)}s do not match.";
    internal const string PasswordDidNotChangeErrorMessage = $"{nameof(Password)} did not change.";

    public ChangePasswordRequestDtoValidator(User user)
    {
        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .SetValidator(new PasswordValidator())
            .Must(x => !user.Authenticate(x))
            .WithMessage(PasswordDidNotChangeErrorMessage);

        RuleFor(x => new { x.Password, x.ConfirmPassword })
            .Must(x => x.Password == x.ConfirmPassword)
            .WithMessage(ConfirmPasswordErrorMessage);
    }

    public ChangePasswordRequestDtoValidator() {}
}