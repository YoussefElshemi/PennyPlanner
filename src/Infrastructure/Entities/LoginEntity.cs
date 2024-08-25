namespace Infrastructure.Entities;

public record LoginEntity : RevokableEntity
{
    public required Guid LoginId { get; init; }
    public required Guid UserId { get; init; }
    public required string IpAddress { get; init; }
    public required string RefreshToken { get; init; }
    public required DateTime ExpiresAt { get; init; }
    public UserEntity UserEntity { get; init; } = null!;
}