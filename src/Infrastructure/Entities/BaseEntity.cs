namespace Infrastructure.Entities;

public record BaseEntity
{
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; set; }
}