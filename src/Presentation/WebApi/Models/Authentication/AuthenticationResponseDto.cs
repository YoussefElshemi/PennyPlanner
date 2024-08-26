using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Models.Authentication;

/// <summary>
///     DTO for the authentication response, containing tokens and expiration details.
/// </summary>
public record AuthenticationResponseDto
{
    /// <summary>
    ///     The unique identifier for the authenticated user.
    /// </summary>
    [Required]
    public required Guid UserId { get; init; }

    /// <summary>
    ///     The type of token issued (e.g., Bearer).
    /// </summary>
    [Required]
    public required string TokenType { get; init; }

    /// <summary>
    ///     The access token issued for the authenticated user.
    /// </summary>
    [Required]
    public required string AccessToken { get; init; }

    /// <summary>
    ///     The duration in seconds until the access token expires.
    /// </summary>
    [Required]
    public required int AccessTokenExpiresIn { get; init; }

    /// <summary>
    ///     The refresh token that can be used to obtain a new access token.
    /// </summary>
    [Required]
    public required string RefreshToken { get; init; }

    /// <summary>
    ///     The duration in seconds until the refresh token expires.
    /// </summary>
    [Required]
    public required int RefreshTokenExpiresIn { get; init; }
}