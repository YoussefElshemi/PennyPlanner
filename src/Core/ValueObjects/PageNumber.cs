namespace Core.ValueObjects;

public readonly record struct PageNumber
{
    private int Value { get; }

    public PageNumber(int pageNumber)
    {
        ArgumentNullException.ThrowIfNull(pageNumber);
        Value = pageNumber;
    }

    public static implicit operator int(PageNumber pageNumber)
    {
        return pageNumber.Value;
    }
}