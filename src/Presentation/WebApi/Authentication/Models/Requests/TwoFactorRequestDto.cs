using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Authentication.Models.Requests;

/// <summary>
///     DTO for submitting two-factor authentication one time passcode.
/// </summary>
public record TwoFactorRequestDto
{
    /// <summary>
    ///     The username of the user attempting to use the code.
    /// </summary>
    [Required]
    public required string Username { get; init; }

    /// <summary>
    ///     The one time passcode that was sent to the user.
    /// </summary>
    [Required]
    public required string Passcode { get; init; }
}