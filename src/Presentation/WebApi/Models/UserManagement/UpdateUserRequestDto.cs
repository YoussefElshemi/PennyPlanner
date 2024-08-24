namespace Presentation.WebApi.Models.UserManagement;

public record UpdateUserRequestDto : User.UpdateUserRequestDto
{
    public required Guid UserId { get; init; }
}