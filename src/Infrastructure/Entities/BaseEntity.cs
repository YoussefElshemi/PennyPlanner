using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public record BaseEntity
{
    [Column(Order = 9998)]
    public required DateTime CreatedAt { get; init; }
    [Column(Order = 9999)]
    public required DateTime UpdatedAt { get; set; }
}