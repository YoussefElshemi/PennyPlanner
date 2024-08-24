namespace Presentation.WebApi.Models.Common;

public record PagedResponseDto<T>
{
    public required PagedResponseMetadataDto Metadata { get; init; }
    public required T[] Data { get; init; }
}