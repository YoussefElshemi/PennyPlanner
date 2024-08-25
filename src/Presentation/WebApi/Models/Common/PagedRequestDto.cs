namespace Presentation.WebApi.Models.Common;

public record PagedRequestDto
{
    public int? PageNumber { get; init; }
    public int? PageSize { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
}