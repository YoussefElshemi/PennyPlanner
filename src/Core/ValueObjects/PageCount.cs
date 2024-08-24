namespace Core.ValueObjects;

public readonly record struct PageCount
{
    internal int Value { get; init; }

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