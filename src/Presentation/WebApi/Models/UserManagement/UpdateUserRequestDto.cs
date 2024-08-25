namespace Presentation.WebApi.Models.UserManagement;

public record UpdateUserRequestDto : AuthenticatedUser.UpdateUserRequestDto
{
    public required Guid UserId { get; init; }
}