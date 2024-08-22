using Core.ValueObjects;

namespace Core.Models;

public record ResetPasswordRequest
{
    public required PasswordResetToken PasswordResetToken { get; init; }
    public required Password Password { get; init; }
}