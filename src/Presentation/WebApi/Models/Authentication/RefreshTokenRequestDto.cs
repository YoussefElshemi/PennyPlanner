using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Models.Authentication;

/// <summary>
/// DTO for requesting a new access token using a refresh token.
/// </summary>
public record RefreshTokenRequestDto
{
    /// <summary>
    /// The refresh token used to obtain a new access token.
    /// </summary>
    [Required]
    public required string RefreshToken { get; init; }
}