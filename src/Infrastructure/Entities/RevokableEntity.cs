namespace Infrastructure.Entities;

public record RevokableEntity : BaseEntity
{
    public required bool IsRevoked { get; set; }
    public required string? RevokedBy { get; set; }
    public required DateTime? RevokedAt { get; set; }
}