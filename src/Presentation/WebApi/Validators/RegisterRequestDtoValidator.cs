using Core.Interfaces.Repositories;
using Core.Validators;
using FluentValidation;
using Presentation.WebApi.Models.Authentication;

namespace Presentation.WebApi.Validators;

public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    internal const string ConfirmPasswordErrorMessage = "Passwords do not match.";
    internal const string UsernameTakenErrorMessage = "Username is taken.";
    internal const string EmailAddressInUseErrorMessage = "Email address is already in use.";

    public RegisterRequestDtoValidator()
    {
        RuleFor(x => x.Username)
            .SetValidator(new UsernameValidator());

        RuleFor(x => x.Password)
            .SetValidator(new PasswordValidator());

        RuleFor(x => new { x.Password, x.ConfirmPassword,})
            .Must(x => x.Password == x.ConfirmPassword)
            .WithMessage(ConfirmPasswordErrorMessage);

        RuleFor(x => x.EmailAddress)
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

        RuleFor(x => new { x.Password, x.ConfirmPassword,})
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