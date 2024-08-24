using Core.ValueObjects;

namespace Core.Models;

public record RefreshTokenRequest
{
    public RefreshToken RefreshToken { get; init; }
    public IpAddress IpAddress { get; init; }
}