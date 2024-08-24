namespace Core.ValueObjects;

public readonly record struct IsRevoked
{
    internal bool Value { get; init; }

    public IsRevoked(bool isRevoked)
    {
        ArgumentNullException.ThrowIfNull(isRevoked);
        Value = isRevoked;
    }

    public static implicit operator bool(IsRevoked isRevoked)
    {
        return isRevoked.Value;
    }
}