namespace Core.ValueObjects;

public readonly record struct IsUsed
{
    internal bool Value { get; init; }

    public IsUsed(bool isUsed)
    {
        ArgumentNullException.ThrowIfNull(isUsed);
        Value = isUsed;
    }

    public static implicit operator bool(IsUsed isUsed)
    {
        return isUsed.Value;
    }
}