namespace Presentation.WebApi.Models.Common;

public record PagedRequestDto
{
    public required int PageNumber { get; init; } = 1;
    public required int PageSize { get; init; } = 10;
}