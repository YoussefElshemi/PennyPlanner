namespace Core.ValueObjects;

public readonly record struct PageSize
{
    private int Value { get; }

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