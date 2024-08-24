namespace Core.ValueObjects;

public readonly record struct DeletedAt
{
    private DateTime Value { get; init; }

    public DeletedAt(DateTime deletedAt)
    {
        ArgumentNullException.ThrowIfNull(deletedAt);
        Value = deletedAt;
    }

    public static implicit operator DateTime(DeletedAt deletedAt)
    {
        return deletedAt.Value;
    }
}