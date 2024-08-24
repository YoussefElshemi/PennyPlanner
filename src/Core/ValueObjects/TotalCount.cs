namespace Core.ValueObjects;

public readonly record struct TotalCount
{
    private int Value { get; }

    public TotalCount(int totalCount)
    {
        ArgumentNullException.ThrowIfNull(totalCount);
        Value = totalCount;
    }

    public static implicit operator int(TotalCount totalCount)
    {
        return totalCount.Value;
    }
}