namespace Infrastructure.Entities;

public record PasswordResetEntity : BaseEntity
{
    public required Guid PasswordResetId { get; init; }
    public required Guid UserId { get; init; }
    public required string ResetToken { get; init; }
    public required DateTime ExpiresAt { get; init; }
    public required bool IsUsed { get; set; }
    public UserEntity UserEntity { get; init; } = null!;
}