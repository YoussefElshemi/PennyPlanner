namespace Core.ValueObjects;

public readonly record struct HasMore
{
    internal bool Value { get; init; }

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