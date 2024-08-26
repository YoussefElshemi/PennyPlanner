using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Authentication.Models.Requests;

/// <summary>
///     DTO for requesting a password reset, containing the user's email address.
/// </summary>
public record RequestResetPasswordRequestDto
{
    /// <summary>
    ///     The email address associated with the user account requesting the password reset.
    /// </summary>
    [Required]
    public required string EmailAddress { get; init; }
}