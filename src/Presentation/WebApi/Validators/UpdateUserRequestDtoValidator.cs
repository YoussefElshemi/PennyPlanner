using Core.Interfaces.Repositories;
using Core.Validators;
using Core.ValueObjects;
using FluentValidation;
using Presentation.WebApi.Models.User;

namespace Presentation.WebApi.Validators;

public class UpdateUserRequestDtoValidator : AbstractValidator<UpdateUserRequestDto>
{
    internal const string EmailAddressInUseErrorMessage = $"{nameof(EmailAddress)} is already in use.";

    public UpdateUserRequestDtoValidator()
    {
        RuleFor(x => x.EmailAddress)
            .SetValidator(new EmailAddressValidator());
    }

    public UpdateUserRequestDtoValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.EmailAddress)
            .SetValidator(new EmailAddressValidator())
            .MustAsync(async (x, _) => await UserNotExistByEmailAddress(userRepository, x))
            .WithMessage(EmailAddressInUseErrorMessage);
    }
    private static async Task<bool> UserNotExistByEmailAddress(IUserRepository userRepository, string emailAddress)
    {
        return !await userRepository.ExistsByEmailAddressAsync(emailAddress);
    }
}