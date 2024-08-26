namespace Presentation.WebApi.Models.AuthenticatedUser;

/// <summary>
///     DTO for updating user information.
/// </summary>
public record UpdateUserRequestDto
{
    /// <summary>
    ///     The new username for the user.
    /// </summary>
    public string? Username { get; init; }

    /// <summary>
    ///     The new email address for the user.
    /// </summary>
    public string? EmailAddress { get; init; }
}