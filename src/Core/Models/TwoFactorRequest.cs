using Core.ValueObjects;

namespace Core.Models;

public record TwoFactorRequest
{
    public required Username Username { get; init; }
    public required Passcode Passcode { get; init; }
    public required IpAddress IpAddress { get; init; }
}