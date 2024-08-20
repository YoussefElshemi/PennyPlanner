namespace Infrastructure.Entities;

public record UserRoleEntity : BaseEntity
{
    public required int UserRoleId { get; init; }
    public required string Name { get; init; }
    public ICollection<UserEntity> Users { get; init; } = new List<UserEntity>();
}