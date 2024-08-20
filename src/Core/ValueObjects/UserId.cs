namespace Core.ValueObjects;

public readonly record struct UserId
{
    internal Guid Value { get; init; }

    public UserId(Guid userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        Value = userId;
    }

    public static implicit operator Guid(UserId userId)
    {
        return userId.Value;
    }

    public static implicit operator string(UserId userId)
    {
        return userId.Value.ToString();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}