using Core.Interfaces.Services;
using Core.Validators;
using Core.ValueObjects;
using FluentValidation;
using Presentation.WebApi.Models.User;

namespace Presentation.WebApi.Validators.User;

public class ChangePasswordRequestDtoValidator : AbstractValidator<ChangePasswordRequestDto>
{
    private readonly IAuthenticationService _authenticationService;

    internal const string ConfirmPasswordErrorMessage = $"{nameof(Password)}s do not match.";
    internal const string PasswordDidNotChangeErrorMessage = $"{nameof(Password)} did not change.";

    public ChangePasswordRequestDtoValidator(IAuthenticationService authenticationService,
        Core.Models.User user)
    {
        _authenticationService = authenticationService;

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .SetValidator(new PasswordValidator())
            .Must(x => !_authenticationService.Authenticate(user, new Password(x)))
            .WithMessage(PasswordDidNotChangeErrorMessage);

        RuleFor(x => new { x.Password, x.ConfirmPassword })
            .Must(x => x.Password == x.ConfirmPassword)
            .WithMessage(ConfirmPasswordErrorMessage);
    }

    public ChangePasswordRequestDtoValidator(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
}