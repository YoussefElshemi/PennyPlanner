using Core.Extensions;
using Core.Interfaces.Repositories;
using Core.Validators;
using Core.ValueObjects;
using FluentValidation;
using Presentation.WebApi.Models.Authentication;

namespace Presentation.WebApi.Validators;

public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    internal const string ConfirmPasswordErrorMessage = $"{nameof(Password)}s do not match.";
    internal const string UsernameTakenErrorMessage = $"{nameof(Username)} is taken.";
    internal const string EmailAddressInUseErrorMessage = $"{nameof(EmailAddress)} is already in use.";

    public RegisterRequestDtoValidator()
    {
        RuleFor(x => x.Username)
            .WithDisplayName(nameof(Username))
            .SetValidator(new UsernameValidator());

        RuleFor(x => x.Password)
            .WithDisplayName(nameof(Password))
            .SetValidator(new PasswordValidator());

        RuleFor(x => new { x.Password, x.ConfirmPassword })
            .WithDisplayName(nameof(Password))
            .Must(x => x.Password == x.ConfirmPassword)
            .WithMessage(ConfirmPasswordErrorMessage);

        RuleFor(x => x.EmailAddress)
            .WithDisplayName(nameof(EmailAddress))
            .SetValidator(new EmailAddressValidator());
    }

    public RegisterRequestDtoValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.Username)
            .SetValidator(new UsernameValidator())
            .MustAsync(async (x, _) => await UserNotExistByUsername(userRepository, x))
            .WithMessage(UsernameTakenErrorMessage);

        RuleFor(x => x.Password)
            .SetValidator(new PasswordValidator());

        RuleFor(x => new { x.Password, x.ConfirmPassword })
            .WithDisplayName(nameof(Password))
            .Must(x => x.Password == x.ConfirmPassword)
            .WithMessage(ConfirmPasswordErrorMessage);

        RuleFor(x => x.EmailAddress)
            .SetValidator(new EmailAddressValidator())
            .MustAsync(async (x, _) => await UserNotExistByEmailAddress(userRepository, x))
            .WithMessage(EmailAddressInUseErrorMessage);
    }


    private static async Task<bool> UserNotExistByUsername(IUserRepository userRepository, string username)
    {
        return !await userRepository.ExistsByUsernameAsync(username);
    }

    private static async Task<bool> UserNotExistByEmailAddress(IUserRepository userRepository, string emailAddress)
    {
        return !await userRepository.ExistsByEmailAddressAsync(emailAddress);
    }
}