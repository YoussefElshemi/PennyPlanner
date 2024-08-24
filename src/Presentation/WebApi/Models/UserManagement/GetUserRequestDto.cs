namespace Presentation.WebApi.Models.UserManagement;

public record GetUserRequestDto
{
    public required Guid UserId { get; init; }
}