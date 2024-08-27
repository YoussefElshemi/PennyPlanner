using Core.ValueObjects;

namespace Core.Models;

public record RequestPasswordResetRequest
{
    public required EmailAddress EmailAddress { get; init; }
}