using Core.ValueObjects;

namespace Core.Models;

public record CreateUserRequest
{
    public required Username Username { get; init; }
    public required Password Password { get; init; }
    public required EmailAddress EmailAddress { get; init; }
}