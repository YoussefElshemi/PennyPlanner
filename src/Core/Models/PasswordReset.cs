using Core.ValueObjects;

namespace Core.Models;

public record PasswordReset
{
    public required PasswordResetId PasswordResetId { get; init; }
    public required UserId UserId { get; init; }
    public required User User { get; init; }
    public required PasswordResetToken ResetToken { get; init; }
    public required IsUsed IsUsed { get; init; }
    public required Username CreatedBy { get; init; }
    public required CreatedAt CreatedAt { get; init; }
    public required Username UpdatedBy { get; init; }
    public required UpdatedAt UpdatedAt { get; init; }
}