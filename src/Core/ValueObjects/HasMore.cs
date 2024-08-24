namespace Core.ValueObjects;

public readonly record struct HasMore
{
    private bool Value { get; }

    public HasMore(bool hasMore)
    {
        ArgumentNullException.ThrowIfNull(hasMore);
        Value = hasMore;
    }

    public static implicit operator bool(HasMore hasMore)
    {
        return hasMore.Value;
    }
}