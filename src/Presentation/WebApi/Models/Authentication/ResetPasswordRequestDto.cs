using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Models.Authentication;

/// <summary>
///     DTO for resetting a user's password, including the reset token and new password details.
/// </summary>
public record ResetPasswordRequestDto
{
    /// <summary>
    ///     The token provided to the user for password reset.
    /// </summary>
    [Required]
    public required string PasswordResetToken { get; init; }

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