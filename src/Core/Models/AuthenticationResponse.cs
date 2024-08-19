using Core.ValueObjects;

namespace Core.Models;

public record AuthenticationResponse
{
    public required UserId UserId { get; init; }
}