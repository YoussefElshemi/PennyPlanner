namespace Core.ValueObjects;

public readonly record struct IsUsed
{
    public IsUsed(bool isUsed)
    {
        ArgumentNullException.ThrowIfNull(isUsed);
        Value = isUsed;
    }

    private bool Value { get; }

    public static implicit operator bool(IsUsed isUsed)
    {
        return isUsed.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}