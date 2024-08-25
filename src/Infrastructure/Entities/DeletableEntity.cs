namespace Infrastructure.Entities;

public record DeletableEntity : BaseEntity
{
    public required bool IsDeleted { get; set; }
    public required string? DeletedBy { get; set; }
    public required DateTime? DeletedAt { get; set; }
}