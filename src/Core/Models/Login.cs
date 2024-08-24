using Core.ValueObjects;

namespace Core.Models;

public record Login
{
    public required LoginId LoginId { get; init; }
    public required UserId UserId { get; init; }
    public required IpAddress IpAddress { get; init; }
    public required RefreshToken RefreshToken { get; init; }
    public required ExpiresAt ExpiresAt { get; init; }
    public required IsRevoked IsRevoked { get; init; }
    public RevokedAt? RevokedAt { get; init; }
    public required CreatedAt CreatedAt { get; init; }
    public required Username UpdatedBy { get; init; }
    public required UpdatedAt UpdatedAt { get; init; }
    public required User User { get; init; }
}