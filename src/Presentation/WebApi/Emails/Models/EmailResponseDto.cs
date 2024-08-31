using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Emails.Models;

public record EmailResponseDto
{
    /// <summary>
    ///     The unique identifier for the email.
    /// </summary>
    [Required]
    public required Guid EmailId { get; init; }

    /// <summary>
    ///     The recipient email address for the email.
    /// </summary>
    [Required]
    public required string EmailAddress { get; init; }

    /// <summary>
    ///     The subject for the email.
    /// </summary>
    [Required]
    public required string EmailSubject { get; init; }

    /// <summary>
    ///     The body for the email.
    /// </summary>
    [Required]
    public required string EmailBody { get; init; }

    /// <summary>
    ///     If the email has been successfully processed.
    /// </summary>
    [Required]
    public required bool IsProcessed { get; init; }

    /// <summary>
    ///     The username of the person who created the email.
    /// </summary>
    [Required]
    public required string CreatedBy { get; init; }

    /// <summary>
    ///     The date and time when the email was created.
    /// </summary>
    [Required]
    public required string CreatedAt { get; init; }

    /// <summary>
    ///     The username of the person who last updated the email.
    /// </summary>
    [Required]
    public required string UpdatedBy { get; init; }

    /// <summary>
    ///     The date and time when the email was last updated.
    /// </summary>
    [Required]
    public required string UpdatedAt { get; init; }
}