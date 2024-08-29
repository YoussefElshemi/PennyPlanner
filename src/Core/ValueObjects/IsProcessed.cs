namespace Core.ValueObjects;

public readonly record struct IsProcessed
{
    public IsProcessed(bool isProcessed)
    {
        ArgumentNullException.ThrowIfNull(isProcessed);
        Value = isProcessed;
    }

    private bool Value { get; }

    public static implicit operator bool(IsProcessed isProcessed)
    {
        return isProcessed.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}