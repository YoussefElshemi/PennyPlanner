using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Models.Common;

/// <summary>
/// Metadata for paginated responses, providing details about pagination and result set.
/// </summary>
public record PagedResponseMetadataDto
{
    /// <summary>
    /// The current page number in the paginated result.
    /// </summary>
    [Required]
    public required int PageNumber { get; init; }

    /// <summary>
    /// The number of items per page.
    /// </summary>
    [Required]
    public required int PageSize { get; init; }

    /// <summary>
    /// The total number of pages available based on the current page size and total count.
    /// </summary>
    [Required]
    public required int PageCount { get; init; }

    /// <summary>
    /// The total number of items available across all pages.
    /// </summary>
    [Required]
    public required int TotalCount { get; init; }

    /// <summary>
    /// Indicates whether there are more pages of data available beyond the current page.
    /// </summary>
    [Required]
    public required bool HasMore { get; init; }
}