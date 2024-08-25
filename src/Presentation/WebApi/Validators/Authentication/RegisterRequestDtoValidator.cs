using System.Net;
using Core.Interfaces.Repositories;
using Core.Validators;
using Core.ValueObjects;
using FluentValidation;
using Presentation.WebApi.Models.Authentication;

namespace Presentation.WebApi.Validators.Authentication;

public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    private readonly IUserRepository _userRepository;
    internal const string ConfirmPasswordErrorMessage = $"{nameof(Password)}s do not match.";
    internal const string UsernameTakenErrorMessage = $"{nameof(Username)} is taken.";
    internal const string EmailAddressInUseErrorMessage = $"{nameof(EmailAddress)} is already in use.";

    public RegisterRequestDtoValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.Username)
            .Cascade(CascadeMode.Stop)
            .SetValidator(new UsernameValidator())
            .MustAsync(async (x, _) => await UserNotExistByUsername(x))
            .WithErrorCode(HttpStatusCode.Conflict.ToString())
            .WithMessage(UsernameTakenErrorMessage);

        RuleFor(x => x.Password)
            .SetValidator(new PasswordValidator());

        RuleFor(x => new { x.Password, x.ConfirmPassword })
            .Must(x => x.Password == x.ConfirmPassword)
            .WithMessage(ConfirmPasswordErrorMessage);

        RuleFor(x => x.EmailAddress)
            .Cascade(CascadeMode.Stop)
            .SetValidator(new EmailAddressValidator())
            .MustAsync(async (x, _) => await UserNotExistByEmailAddress(x))
            .WithErrorCode(HttpStatusCode.Conflict.ToString())
            .WithMessage(EmailAddressInUseErrorMessage);
    }

    private async Task<bool> UserNotExistByUsername(string username)
    {
        return !await _userRepository.ExistsByUsernameAsync(username);
    }

    private async Task<bool> UserNotExistByEmailAddress(string emailAddress)
    {
        return !await _userRepository.ExistsByEmailAddressAsync(emailAddress);
    }
}