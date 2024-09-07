using System.Net;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.ValueObjects;
using FluentValidation;
using Presentation.WebApi.Authentication.Models.Requests;

namespace Presentation.WebApi.Authentication.Validators;

public class TwoFactorRequestDtoValidator : AbstractValidator<TwoFactorRequestDto>
{
    internal const string UserNotFoundErrorMessage = $"{nameof(User)} not found.";
    internal const string PasscodeNotFoundErrorMessage = $"{nameof(Passcode)} not found.";
    internal const string PasscodeAlreadyUsedErrorMessage = $"{nameof(Passcode)} already used.";
    internal const string PasscodeExpiredErrorMessage = $"{nameof(Passcode)} has expired.";

    private readonly IOneTimePasscodeRepository _oneTimePasscodeRepository;
    private readonly IUserRepository _userRepository;
    private readonly TimeProvider _timeProvider;

    public TwoFactorRequestDtoValidator(IOneTimePasscodeRepository oneTimePasscodeRepository,
        IUserRepository userRepository,
        TimeProvider timeProvider)
    {
        _oneTimePasscodeRepository = oneTimePasscodeRepository;
        _userRepository = userRepository;
        _timeProvider = timeProvider;

        RuleFor(x => x.Username)
            .NotEmpty();

        RuleFor(x => x.Passcode)
            .NotEmpty();

        RuleFor(x => new { x.Username, x.Passcode })
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (x, _) => await UserExistByUsername(x.Username))
            .WithErrorCode(HttpStatusCode.NotFound.ToString())
            .WithMessage(UserNotFoundErrorMessage)
            .MustAsync(async (x, _) => await TwoFactorRequestExists(x.Username, x.Passcode))
            .WithErrorCode(HttpStatusCode.NotFound.ToString())
            .WithMessage(PasscodeNotFoundErrorMessage)
            .MustAsync(async (x, _) => await TwoFactorRequestNotUsed(x.Username, x.Passcode))
            .WithErrorCode(HttpStatusCode.Conflict.ToString())
            .WithMessage(PasscodeAlreadyUsedErrorMessage)
            .MustAsync(async (x, _) => await TwoFactorRequestNotExpired(x.Username, x.Passcode))
            .WithErrorCode(HttpStatusCode.Gone.ToString())
            .WithMessage(PasscodeExpiredErrorMessage);
    }

    private Task<bool> UserExistByUsername(string username)
    {
        return _userRepository.ExistsByUsernameAsync(username);
    }

    private async Task<bool> TwoFactorRequestExists(string username, string passcode)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        return await _oneTimePasscodeRepository.ExistsAsync(user.UserId, passcode);
    }

    private async Task<bool> TwoFactorRequestNotUsed(string username, string passcode)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        var oneTimePasscode = await _oneTimePasscodeRepository.GetAsync(user.UserId, passcode);

        return !oneTimePasscode.IsUsed;
    }

    private async Task<bool> TwoFactorRequestNotExpired(string username, string passcode)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        var oneTimePasscode = await _oneTimePasscodeRepository.GetAsync(user.UserId, passcode);

        return oneTimePasscode.ExpiresAt >= _timeProvider.GetUtcNow().UtcDateTime;
    }
}