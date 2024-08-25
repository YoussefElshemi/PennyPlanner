namespace Core.ValueObjects;

public readonly record struct IsRevoked
{
    public IsRevoked(bool isRevoked)
    {
        ArgumentNullException.ThrowIfNull(isRevoked);
        Value = isRevoked;
    }

    private bool Value { get; }

    public static implicit operator bool(IsRevoked isRevoked)
    {
        return isRevoked.Value;
    }
}