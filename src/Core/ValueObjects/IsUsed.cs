namespace Core.ValueObjects;

public readonly record struct IsUsed
{
    private bool Value { get; }

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