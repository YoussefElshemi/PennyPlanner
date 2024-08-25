namespace Presentation.WebApi.Models.Common;

public record PagedResponseMetadataDto
{
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
    public required int PageCount { get; init; }
    public required int TotalCount { get; init; }
    public required bool HasMore { get; init; }
}