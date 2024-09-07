using Core.ValueObjects;

namespace Core.Models;

public record OneTimePasscode
{
    public required OneTimePasscodeId OneTimePasscodeId { get; init; }
    public required UserId UserId { get; init; }
    public required IpAddress IpAddress { get; init; }
    public required Passcode Passcode { get; init; }
    public required IsUsed IsUsed { get; init; }
    public required ExpiresAt ExpiresAt { get; init; }
    public required Username CreatedBy { get; init; }
    public required CreatedAt CreatedAt { get; init; }
    public required Username UpdatedBy { get; init; }
    public required UpdatedAt UpdatedAt { get; init; }
    public required User User { get; init; }
}