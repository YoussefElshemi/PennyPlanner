using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Models.UserManagement;

/// <summary>
/// DTO for specifying a user by their unique identifier.
/// </summary>
public record UserRequestDto
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    [Required]
    public required Guid UserId { get; init; }
}