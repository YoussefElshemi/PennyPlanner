namespace Infrastructure.Entities;

public record UserEntity : BaseEntity
{
    public required Guid Id { get; init; }
    public required string Username { get; init; }
    public required string EmailAddress { get; init; }
    public required string PasswordHash { get; init; }
}