using Core.Enums;
using Core.ValueObjects;

namespace Core.Models;

public record AuthenticationResponse
{
    public required UserId UserId { get; init; }
    public required TokenType TokenType { get; init; }
    public required AccessToken AccessToken { get; init; }
    public required ExpiresIn AccessTokenExpiresIn { get; init; }
    public required RefreshToken RefreshToken { get; init; }
    public required ExpiresIn RefreshTokenExpiresIn { get; init; }
}