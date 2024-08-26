using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Common.Models.Responses;

/// <summary>
///     DTO for paginated responses, including metadata and data for a specific type.
/// </summary>
/// <typeparam name="T">The type of the data items in the response.</typeparam>
public record PagedResponseDto<T>
{
    /// <summary>
    ///     Metadata about the pagination, such as total count and current page details.
    /// </summary>
    [Required]
    public required PagedResponseMetadataDto Metadata { get; init; }

    /// <summary>
    ///     The data items for the current page.
    /// </summary>
    [Required]
    public required T[] Data { get; init; }
}