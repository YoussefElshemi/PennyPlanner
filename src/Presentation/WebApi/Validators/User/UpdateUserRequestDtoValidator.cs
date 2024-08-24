using Core.Extensions;
using Core.Interfaces.Repositories;
using Core.Validators;
using Core.ValueObjects;
using FluentValidation;
using Presentation.WebApi.Models.User;

namespace Presentation.WebApi.Validators.User;

public class UpdateUserRequestDtoValidator : AbstractValidator<UpdateUserRequestDto>
{
    private readonly IUserRepository _userRepository;
    internal const string AtLeastOneFieldProvidedErrorMessage = "One or more fields are missing.";
    internal const string FieldDidNotUpdateErrorMessage = "Field is the same as the current value.";
    internal const string EmailAddressInUseErrorMessage = $"{nameof(EmailAddress)} is already in use.";
    internal const string UsernameInUseErrorMessage = $"{nameof(Username)} is already in use.";

    public UpdateUserRequestDtoValidator(IUserRepository userRepository, Core.Models.User user)
    {
        _userRepository = userRepository;

        RuleFor(x => x)
            .WithDisplayName("Fields")
            .Must(AtLeastOneFieldProvided)
            .WithMessage(AtLeastOneFieldProvidedErrorMessage)
            .DependentRules(() =>
            {
                RuleFor(x => x.EmailAddress)
                    .Cascade(CascadeMode.Stop)
                    .SetValidator(new EmailAddressValidator()!)
                    .Must(x => x != user.EmailAddress)
                    .WithMessage(FieldDidNotUpdateErrorMessage)
                    .MustAsync(async (x, _) => await UserNotExistByEmailAddress(x!))
                    .WithMessage(EmailAddressInUseErrorMessage)
                    .When(x => x.EmailAddress != null);

                RuleFor(x => x.Username)
                    .Cascade(CascadeMode.Stop)
                    .SetValidator(new UsernameValidator()!)
                    .Must(x => x != user.Username)
                    .WithMessage(FieldDidNotUpdateErrorMessage)
                    .MustAsync(async (x, _) => await UserNotExistByUsername(x!))
                    .WithMessage(UsernameInUseErrorMessage)
                    .When(x => x.Username != null);
            });
    }
    private async Task<bool> UserNotExistByEmailAddress(string emailAddress)
    {
        return !await _userRepository.ExistsByEmailAddressAsync(emailAddress);
    }

    private async Task<bool> UserNotExistByUsername(string username)
    {
        return !await _userRepository.ExistsByUsernameAsync(username);
    }

    private static bool AtLeastOneFieldProvided(UpdateUserRequestDto request)
    {
        return
            request.EmailAddress != null ||
            request.Username != null;
    }

    public UpdateUserRequestDtoValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
}