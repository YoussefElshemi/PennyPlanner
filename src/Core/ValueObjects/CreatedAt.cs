namespace Core.ValueObjects;

public record struct CreatedAt
{
    private DateTime Value { get; init; }

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