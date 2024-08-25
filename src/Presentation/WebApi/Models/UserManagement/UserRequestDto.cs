namespace Presentation.WebApi.Models.UserManagement;

public record UserRequestDto
{
    public required Guid UserId { get; init; }
}