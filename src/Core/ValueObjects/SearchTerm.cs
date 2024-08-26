namespace Core.ValueObjects;

public readonly record struct SearchTerm
{
    public SearchTerm(string searchTerm)
    {
        ArgumentNullException.ThrowIfNull(searchTerm);
        Value = searchTerm;
    }

    private string Value { get; }

    public static implicit operator string(SearchTerm searchTerm)
    {
        return searchTerm.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}