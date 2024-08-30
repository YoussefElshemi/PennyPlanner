namespace Presentation.WebApi.Email.Models;

public record RedriveEmailRequestDto
{
    public required Guid EmailId { get; init; }
}