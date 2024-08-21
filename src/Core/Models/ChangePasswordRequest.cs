using Core.ValueObjects;

namespace Core.Models;

public record ChangePasswordRequest
{
    public required Password Password { get; init; }
}