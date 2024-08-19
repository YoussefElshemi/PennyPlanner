namespace Infrastructure.Entities;

public record BaseEntity
{
    public required Guid CreatedBy { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
}