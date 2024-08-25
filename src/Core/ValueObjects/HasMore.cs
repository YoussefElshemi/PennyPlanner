namespace Core.ValueObjects;

public readonly record struct HasMore
{
    public HasMore(bool hasMore)
    {
        ArgumentNullException.ThrowIfNull(hasMore);
        Value = hasMore;
    }

    private bool Value { get; }

    public static implicit operator bool(HasMore hasMore)
    {
        return hasMore.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}