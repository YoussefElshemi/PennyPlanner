namespace Core.ValueObjects;

public readonly record struct SortBy
{
    public SortBy(string sortBy)
    {
        ArgumentNullException.ThrowIfNull(sortBy);
        Value = sortBy;
    }

    private string Value { get; }

    public static implicit operator string(SortBy sortBy)
    {
        return sortBy.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}