namespace Core.ValueObjects;

public readonly record struct TotalCount
{
    public TotalCount(int totalCount)
    {
        ArgumentNullException.ThrowIfNull(totalCount);
        Value = totalCount;
    }

    private int Value { get; }

    public static implicit operator int(TotalCount totalCount)
    {
        return totalCount.Value;
    }
}