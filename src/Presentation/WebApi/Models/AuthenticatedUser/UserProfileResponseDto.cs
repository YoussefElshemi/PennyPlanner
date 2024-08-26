using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Models.AuthenticatedUser;

/// <summary>
///     DTO for user profile information response.
/// </summary>
public record UserProfileResponseDto
{
    /// <summary>
    ///     The unique identifier for the user.
    /// </summary>
    [Required]
    public required Guid UserId { get; init; }

    /// <summary>
    ///     The username of the user.
    /// </summary>
    [Required]
    public required string Username { get; init; }

    /// <summary>
    ///     The email address of the user.
    /// </summary>
    [Required]
    public required string EmailAddress { get; init; }

    /// <summary>
    ///     The role assigned to the user.
    /// </summary>
    [Required]
    public required string UserRole { get; init; }

    /// <summary>
    ///     The username of the person who created the user profile.
    /// </summary>
    [Required]
    public required string CreatedBy { get; init; }

    /// <summary>
    ///     The date and time when the user profile was created.
    /// </summary>
    [Required]
    public required string CreatedAt { get; init; }

    /// <summary>
    ///     The username of the person who last updated the user profile.
    /// </summary>
    [Required]
    public required string UpdatedBy { get; init; }

    /// <summary>
    ///     The date and time when the user profile was last updated.
    /// </summary>
    [Required]
    public required string UpdatedAt { get; init; }
}