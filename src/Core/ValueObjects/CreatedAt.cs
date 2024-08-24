namespace Core.ValueObjects;

public readonly record struct CreatedAt
{
    private DateTime Value { get; }

    public CreatedAt(DateTime createdAt)
    {
        ArgumentNullException.ThrowIfNull(createdAt);
        Value = createdAt;
    }

    public static implicit operator DateTime(CreatedAt createdAt)
    {
        return createdAt.Value;
    }
}