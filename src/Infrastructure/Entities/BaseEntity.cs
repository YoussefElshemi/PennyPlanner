namespace Infrastructure.Entities;

public record BaseEntity
{
    public required string CreatedBy { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string UpdatedBy { get; set; }
    public required DateTime UpdatedAt { get; set; }
}