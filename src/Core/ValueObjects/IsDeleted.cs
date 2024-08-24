namespace Core.ValueObjects;

public readonly record struct IsDeleted
{
    private bool Value { get; }

    public IsDeleted(bool isDeleted)
    {
        ArgumentNullException.ThrowIfNull(isDeleted);
        Value = isDeleted;
    }

    public static implicit operator bool(IsDeleted isDeleted)
    {
        return isDeleted.Value;
    }
}