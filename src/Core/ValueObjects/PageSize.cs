namespace Core.ValueObjects;

public readonly record struct PageSize
{
    public PageSize(int pageSize)
    {
        ArgumentNullException.ThrowIfNull(pageSize);
        Value = pageSize;
    }

    private int Value { get; }

    public static implicit operator int(PageSize pageSize)
    {
        return pageSize.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}