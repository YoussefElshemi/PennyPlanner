namespace Infrastructure.Entities;

public record OneTimePasscodeEntity : BaseEntity
{
    public required Guid OneTimePasscodeId { get; init; }
    public required Guid UserId { get; init; }
    public required string IpAddress { get; init; }
    public required string Passcode { get; init; }
    public required bool IsUsed { get; set; }
    public required DateTime ExpiresAt { get; init; }
    public UserEntity UserEntity { get; init; } = null!;
}