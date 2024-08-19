using Core.Enums;
using Core.ValueObjects;

namespace Core.Models;

public record AuthenticationResponse
{
    public required UserId UserId { get; init; }
    public required AuthenticationToken Token { get; init; }
    public required TokenType TokenType { get; init; }
}