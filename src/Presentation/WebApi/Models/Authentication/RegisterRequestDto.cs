using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Models.Authentication;

/// <summary>
/// DTO for user registration request containing username, password, and email address.
/// </summary>
public record RegisterRequestDto
{
    /// <summary>
    /// The desired username for the new user account.
    /// </summary>
    [Required]
    public required string Username { get; init; }

    /// <summary>
    /// The password for the new user account.
    /// </summary>
    [Required]
    public required string Password { get; init; }

    /// <summary>
    /// Confirmation of the password to ensure it was entered correctly.
    /// </summary>
    [Required]
    public required string ConfirmPassword { get; init; }

    /// <summary>
    /// The email address for the new user account.
    /// </summary>
    [Required]
    public required string EmailAddress { get; init; }
}