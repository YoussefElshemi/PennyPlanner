namespace Core.ValueObjects;

public readonly record struct PageCount
{
    private int Value { get; }

    public PageCount(int pageCount)
    {
        ArgumentNullException.ThrowIfNull(pageCount);
        Value = pageCount;
    }

    public static implicit operator int(PageCount pageCount)
    {
        return pageCount.Value;
    }
}