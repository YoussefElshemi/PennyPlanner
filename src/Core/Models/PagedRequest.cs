using Core.ValueObjects;

namespace Core.Models;

public record PagedRequest
{
    public required PageNumber PageNumber { get; init; } = new(1);
    public required PageSize PageSize { get; init; } = new(10);
}