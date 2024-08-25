namespace Core.ValueObjects;

public readonly record struct UserId
{
    public UserId(Guid userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        Value = userId;
    }

    internal Guid Value { get; }

    public static implicit operator Guid(UserId userId)
    {
        return userId.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}