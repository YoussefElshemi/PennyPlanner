namespace Core.ValueObjects;

public record struct UserId
{
    private Guid Value { get; init; }

    public UserId(Guid userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        Value = userId;
    }

    public static implicit operator Guid(UserId userId)
    {
        return userId.Value;
    }
}