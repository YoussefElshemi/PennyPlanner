namespace Core.ValueObjects;

public readonly record struct CreatedAt
{
    public CreatedAt(DateTime createdAt)
    {
        ArgumentNullException.ThrowIfNull(createdAt);
        Value = createdAt;
    }

    private DateTime Value { get; }

    public static implicit operator DateTime(CreatedAt createdAt)
    {
        return createdAt.Value;
    }
}