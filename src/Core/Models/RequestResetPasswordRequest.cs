using Core.ValueObjects;

namespace Core.Models;

public record RequestResetPasswordRequest
{
    public required EmailAddress EmailAddress { get; init; }
}