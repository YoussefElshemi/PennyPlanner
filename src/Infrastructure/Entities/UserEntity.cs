namespace Infrastructure.Entities;

public record UserEntity : BaseEntity
{
    public required Guid UserId { get; init; }
    public required string Username { get; init; }
    public required string EmailAddress { get; init; }
    public required string PasswordHash { get; init; }
    public required string PasswordSalt { get; init; }
    public required int UserRoleId { get; init; }
    public UserRoleEntity UserRoleEntity { get; init; } = null!;
}