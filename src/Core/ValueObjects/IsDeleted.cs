namespace Core.ValueObjects;

public readonly record struct IsDeleted
{
    public IsDeleted(bool isDeleted)
    {
        ArgumentNullException.ThrowIfNull(isDeleted);
        Value = isDeleted;
    }

    private bool Value { get; }

    public static implicit operator bool(IsDeleted isDeleted)
    {
        return isDeleted.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}