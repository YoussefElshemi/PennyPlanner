namespace Infrastructure.Entities;

public record UserEntity : BaseEntity
{
    public required Guid UserId { get; init; }
    public required string Username { get; set; }
    public required string EmailAddress { get; set; }
    public required string PasswordHash { get; set; }
    public required string PasswordSalt { get; set; }
    public required int UserRoleId { get; set; }
    public required bool IsDeleted { get; set; }
    public required string? DeletedBy { get; set; }
    public required DateTime? DeletedAt { get; set; }

    public UserRoleEntity UserRoleEntity { get; init; } = null!;
    public ICollection<PasswordResetEntity> PasswordResets { get; init; } = new List<PasswordResetEntity>();
    public ICollection<LoginEntity> Logins { get; init; } = new List<LoginEntity>();
}