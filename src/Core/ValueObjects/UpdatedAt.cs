namespace Core.ValueObjects;

public readonly record struct UpdatedAt
{
    public UpdatedAt(DateTime updatedAt)
    {
        ArgumentNullException.ThrowIfNull(updatedAt);
        Value = updatedAt;
    }

    private DateTime Value { get; }

    public static implicit operator DateTime(UpdatedAt updatedAt)
    {
        return updatedAt.Value;
    }
}