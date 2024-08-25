namespace Core.ValueObjects;

public readonly record struct PageCount
{
    public PageCount(int pageCount)
    {
        ArgumentNullException.ThrowIfNull(pageCount);
        Value = pageCount;
    }

    private int Value { get; }

    public static implicit operator int(PageCount pageCount)
    {
        return pageCount.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}