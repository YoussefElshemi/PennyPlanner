using System.Net;
using Core.Interfaces.Services;
using Core.Models;
using Core.Validators;
using Core.ValueObjects;
using FluentValidation;
using Presentation.WebApi.AuthenticatedUser.Models.Requests;

namespace Presentation.WebApi.AuthenticatedUser.Validators;

public class ChangePasswordRequestDtoValidator : AbstractValidator<ChangePasswordRequestDto>
{
    internal const string ConfirmPasswordErrorMessage = $"{nameof(Password)}s do not match.";
    internal const string PasswordDidNotChangeErrorMessage = $"{nameof(Password)} did not change.";
    internal const string PasswordIncorrectErrorMessage = $"{nameof(Password)} is incorrect.";

    public ChangePasswordRequestDtoValidator(IAuthenticationService authenticationService,
        User user)
    {
        RuleFor(x => x.CurrentPassword)
            .Must(x => authenticationService.Authenticate(user, new Password(x)))
            .WithErrorCode(HttpStatusCode.Unauthorized.ToString())
            .WithMessage(PasswordIncorrectErrorMessage)
            .DependentRules(() =>
            {
                RuleFor(x => x.Password)
                    .Cascade(CascadeMode.Stop)
                    .SetValidator(new PasswordValidator())
                    .Must(x => !authenticationService.Authenticate(user, new Password(x)))
                    .WithErrorCode(HttpStatusCode.Conflict.ToString())
                    .WithMessage(PasswordDidNotChangeErrorMessage);

                RuleFor(x => new { x.Password, x.ConfirmPassword })
                    .Must(x => x.Password == x.ConfirmPassword)
                    .WithMessage(ConfirmPasswordErrorMessage);
            });
    }

    public ChangePasswordRequestDtoValidator(IAuthenticationService authenticationService)
    {
    }
}