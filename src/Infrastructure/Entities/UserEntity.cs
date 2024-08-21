namespace Infrastructure.Entities;

public record UserEntity : BaseEntity
{
    public required Guid UserId { get; init; }
    public required string Username { get; init; }
    public required string EmailAddress { get; set; }
    public required string PasswordHash { get; set; }
    public required string PasswordSalt { get; set; }
    public required int UserRoleId { get; set; }
    public UserRoleEntity UserRoleEntity { get; init; } = null!;
}