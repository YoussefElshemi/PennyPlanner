namespace Core.ValueObjects;

public readonly record struct IsRevoked
{
    private bool Value { get; }

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