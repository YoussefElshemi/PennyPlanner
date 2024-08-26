using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Models.Authentication;

/// <summary>
/// DTO for user login request containing username and password.
/// </summary>
public record LoginRequestDto
{
    /// <summary>
    /// The username of the user attempting to log in.
    /// </summary>
    [Required]
    public required string Username { get; init; }

    /// <summary>
    /// The password of the user attempting to log in.
    /// </summary>
    [Required]
    public required string Password { get; init; }
}