namespace Presentation.WebApi.Models.Common;

/// <summary>
///     DTO for handling paginated requests, including sorting and searching options.
/// </summary>
public record PagedRequestDto
{
    /// <summary>
    ///     The page number to retrieve, starting from 1.
    /// </summary>
    public int? PageNumber { get; init; }

    /// <summary>
    ///     The number of items to include per page.
    /// </summary>
    public int? PageSize { get; init; }

    /// <summary>
    ///     The field by which to sort the results.
    /// </summary>
    public string? SortBy { get; init; }

    /// <summary>
    ///     The order in which to sort the results.
    /// </summary>
    public string? SortOrder { get; init; }

    /// <summary>
    ///     The specific field to search within.
    /// </summary>
    public string? SearchField { get; init; }

    /// <summary>
    ///     The term to search for in the results.
    /// </summary>
    public string? SearchTerm { get; init; }
}