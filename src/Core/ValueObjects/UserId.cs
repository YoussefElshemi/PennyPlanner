namespace Core.ValueObjects;

public readonly record struct UserId
{
    internal Guid Value { get; }

    public UserId(Guid userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        Value = userId;
    }

    public static implicit operator Guid(UserId userId)
    {
        return userId.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}