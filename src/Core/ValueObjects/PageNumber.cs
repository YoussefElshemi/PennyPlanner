namespace Core.ValueObjects;

public readonly record struct PageNumber
{
    public PageNumber(int pageNumber)
    {
        ArgumentNullException.ThrowIfNull(pageNumber);
        Value = pageNumber;
    }

    private int Value { get; }

    public static implicit operator int(PageNumber pageNumber)
    {
        return pageNumber.Value;
    }
}