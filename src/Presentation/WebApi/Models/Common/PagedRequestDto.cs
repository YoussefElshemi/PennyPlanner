namespace Presentation.WebApi.Models.Common;

public record PagedRequestDto
{
    public int? PageNumber { get; init; }
    public int? PageSize { get; init; }
}