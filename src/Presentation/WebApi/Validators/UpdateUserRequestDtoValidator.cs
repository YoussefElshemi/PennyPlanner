using Core.Interfaces.Repositories;
using Core.Validators;
using Core.ValueObjects;
using FluentValidation;
using Presentation.WebApi.Models.User;

namespace Presentation.WebApi.Validators;

public class UpdateUserRequestDtoValidator : AbstractValidator<UpdateUserRequestDto>
{
    private readonly IUserRepository _userRepository;
    internal const string EmailAddressInUseErrorMessage = $"{nameof(EmailAddress)} is already in use.";

    public UpdateUserRequestDtoValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.EmailAddress)
            .SetValidator(new EmailAddressValidator())
            .MustAsync(async (x, _) => await UserNotExistByEmailAddress(x))
            .WithMessage(EmailAddressInUseErrorMessage);
    }
    private async Task<bool> UserNotExistByEmailAddress(string emailAddress)
    {
        return !await _userRepository.ExistsByEmailAddressAsync(emailAddress);
    }
}