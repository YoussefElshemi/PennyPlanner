using System.Net;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Validators;
using Core.ValueObjects;
using FluentValidation;
using Presentation.WebApi.AuthenticatedUser.Models.Requests;

namespace Presentation.WebApi.AuthenticatedUser.Validators;

public class UpdateUserRequestDtoValidator : AbstractValidator<UpdateUserRequestDto>
{
    internal const string EmailAddressInUseErrorMessage = $"{nameof(EmailAddress)} is already in use.";
    internal const string UsernameInUseErrorMessage = $"{nameof(Username)} is already in use.";
    internal static readonly Func<string, string> FieldDidNotUpdateErrorMessage = field => $"{field} is the same as the current value.";

    private readonly IUserRepository _userRepository;

    public UpdateUserRequestDtoValidator(IUserRepository userRepository, User user)
    {
        _userRepository = userRepository;

        RuleFor(x => x.EmailAddress)
            .Cascade(CascadeMode.Stop)
            .SetValidator(new EmailAddressValidator()!)
            .Must(x => x != user.EmailAddress)
            .WithErrorCode(HttpStatusCode.Conflict.ToString())
            .WithMessage(FieldDidNotUpdateErrorMessage(nameof(EmailAddress)))
            .MustAsync(async (x, _) => await UserNotExistByEmailAddress(x!))
            .WithErrorCode(HttpStatusCode.Conflict.ToString())
            .WithMessage(EmailAddressInUseErrorMessage)
            .When(x => x.EmailAddress != null);

        RuleFor(x => x.Username)
            .Cascade(CascadeMode.Stop)
            .SetValidator(new UsernameValidator()!)
            .Must(x => x != user.Username)
            .WithErrorCode(HttpStatusCode.Conflict.ToString())
            .WithMessage(FieldDidNotUpdateErrorMessage(nameof(Username)))
            .MustAsync(async (x, _) => await UserNotExistByUsername(x!))
            .WithErrorCode(HttpStatusCode.Conflict.ToString())
            .WithMessage(UsernameInUseErrorMessage)
            .When(x => x.Username != null);
    }

    public UpdateUserRequestDtoValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    private async Task<bool> UserNotExistByEmailAddress(string emailAddress)
    {
        return !await _userRepository.ExistsByEmailAddressAsync(emailAddress);
    }

    private async Task<bool> UserNotExistByUsername(string username)
    {
        return !await _userRepository.ExistsByUsernameAsync(username);
    }
}