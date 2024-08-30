namespace Presentation.WebApi.Email.Models;

public record EmailResponseDto
{
    public required Guid EmailId { get; init; }
    public required bool IsProcessed { get; set; }
}