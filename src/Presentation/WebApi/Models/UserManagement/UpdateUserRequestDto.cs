using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Models.UserManagement;

/// <summary>
///     DTO for updating user information, including a unique identifier for the user.
/// </summary>
public record UpdateUserRequestDto : AuthenticatedUser.UpdateUserRequestDto
{
    /// <summary>
    ///     The unique identifier for the user being updated.
    /// </summary>
    [Required]
    public required Guid UserId { get; init; }
}