using Core.ValueObjects;

namespace Core.Models;

public record AuthenticationRequest
{
    public required Username Username { get; init; }
    public required Password Password { get; init; }
    public required IpAddress IpAddress { get; set; }
}