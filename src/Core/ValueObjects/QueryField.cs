namespace Core.ValueObjects;

public readonly record struct QueryField
{
    public QueryField(string sortBy)
    {
        ArgumentNullException.ThrowIfNull(sortBy);
        Value = sortBy;
    }

    private string Value { get; }

    public static implicit operator string(QueryField queryField)
    {
        return queryField.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}