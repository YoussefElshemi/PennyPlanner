namespace Infrastructure.Entities;

public record LoginEntity : BaseEntity
{
    public required Guid LoginId { get; init; }
    public required Guid UserId { get; init; }
    public required string IpAddress { get; init; }
    public required string RefreshToken { get; init; }
    public required DateTime ExpiresAt { get; init; }
    public required bool IsRevoked { get; set; }
    public required DateTime? RevokedAt { get; set; }
    public UserEntity UserEntity { get; init; } = null!;
}