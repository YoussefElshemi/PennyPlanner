using Core.ValueObjects;

namespace Core.Models;

public record AuthenticationRequest
{
    public required Username Username { get; set; }
    public required Password Password { get; set; }
}