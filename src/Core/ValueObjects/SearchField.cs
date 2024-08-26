namespace Core.ValueObjects;

public readonly record struct SearchField
{
    public SearchField(string searchField)
    {
        ArgumentNullException.ThrowIfNull(searchField);
        Value = searchField;
    }

    private string Value { get; }

    public static implicit operator string(SearchField searchField)
    {
        return searchField.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}