namespace Core.ValueObjects;

public readonly record struct DeletedAt
{
    public DeletedAt(DateTime deletedAt)
    {
        ArgumentNullException.ThrowIfNull(deletedAt);
        Value = deletedAt;
    }

    private DateTime Value { get; }

    public static implicit operator DateTime(DeletedAt deletedAt)
    {
        return deletedAt.Value;
    }
}