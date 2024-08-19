using Core.Enums;
using Core.ValueObjects;

namespace Core.Models;

public record User
{
    public required UserId UserId { get; init; }
    public required Username Username { get; init; }
    public required EmailAddress EmailAddress { get; init; }
    public required PasswordHash PasswordHash { get; init; }
    public required UserRole UserRole { get; init; }
    public required CreatedBy CreatedBy { get; init; }
    public required CreatedAt CreatedAt { get; init; }
    public required UpdatedAt UpdatedAt { get; init; }
}