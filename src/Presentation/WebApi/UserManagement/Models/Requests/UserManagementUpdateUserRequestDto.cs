using Presentation.WebApi.AuthenticatedUser.Models.Requests;

namespace Presentation.WebApi.UserManagement.Models.Requests;

/// <summary>
///     DTO for updating user information, including a unique identifier for the user.
/// </summary>
public record UserManagementUpdateUserRequestDto : UpdateUserRequestDto
{
    /// <summary>
    ///     The unique identifier for the user being updated.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    ///     The unique identifier for the user being updated.
    /// </summary>
    public string? UserRole { get; init; }
}