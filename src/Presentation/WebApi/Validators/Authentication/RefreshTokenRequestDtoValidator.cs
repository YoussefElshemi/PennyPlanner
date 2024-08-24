using Core.Interfaces.Repositories;
using Core.ValueObjects;
using FluentValidation;
using Presentation.WebApi.Models.Authentication;

namespace Presentation.WebApi.Validators.Authentication;

public class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshTokenRequestDto>
{
    private readonly ILoginRepository _loginRepository;
    private readonly TimeProvider _timeProvider;
    internal const string RefreshTokenDoesNotExistErrorMessage = $"{nameof(RefreshToken)} does not exist.";
    internal const string RefreshTokenIsRevokedErrorMessage = $"{nameof(RefreshToken)} is revoked.";
    internal const string RefreshTokenIsExpiredErrorMessage = $"{nameof(RefreshToken)} is expired.";

    public RefreshTokenRequestDtoValidator(
        ILoginRepository loginRepository,
        TimeProvider timeProvider)
    {
        _loginRepository = loginRepository;
        _timeProvider = timeProvider;

        RuleFor(x => x.RefreshToken)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (x, _) => await RefreshTokenExists(x))
            .WithMessage(RefreshTokenDoesNotExistErrorMessage)
            .CustomAsync(async (x, ctx, _) => await RefreshTokenIsValid(x, ctx));
    }

    private Task<bool> RefreshTokenExists(string refreshToken)
    {
        return _loginRepository.ExistsAsync(refreshToken);
    }

    private async Task RefreshTokenIsValid(string refreshToken, ValidationContext<RefreshTokenRequestDto> context)
    {
        var login = await _loginRepository.GetAsync(refreshToken);
        if (login.ExpiresAt < _timeProvider.GetUtcNow().UtcDateTime)
        {
            context.AddFailure(RefreshTokenIsExpiredErrorMessage);
            return;
        }

        if (login.IsRevoked)
        {
            context.AddFailure(RefreshTokenIsRevokedErrorMessage);
        }
    }
}