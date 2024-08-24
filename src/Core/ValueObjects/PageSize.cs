namespace Core.ValueObjects;

public readonly record struct PageSize
{
    internal int Value { get; init; }

    public PageSize(int pageSize)
    {
        ArgumentNullException.ThrowIfNull(pageSize);
        Value = pageSize;
    }

    public static implicit operator int(PageSize pageSize)
    {
        return pageSize.Value;
    }
}