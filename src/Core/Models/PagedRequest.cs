using Core.ValueObjects;

namespace Core.Models;

public record PagedRequest
{
    public required PageNumber PageNumber { get; init; }
    public required PageSize PageSize { get; init; }
}