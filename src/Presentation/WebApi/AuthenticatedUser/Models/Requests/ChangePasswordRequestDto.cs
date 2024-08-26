using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.AuthenticatedUser.Models.Requests;

/// <summary>
///     DTO for changing the user's password.
/// </summary>
public record ChangePasswordRequestDto
{
    /// <summary>
    ///     The new password the user wants to set.
    /// </summary>
    [Required]
    public required string Password { get; init; }

    /// <summary>
    ///     Confirmation of the new password to ensure it was entered correctly.
    /// </summary>
    [Required]
    public required string ConfirmPassword { get; init; }
}