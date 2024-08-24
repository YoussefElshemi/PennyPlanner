namespace Core.ValueObjects;

public readonly record struct UpdatedAt
{
    private DateTime Value { get; }

    public UpdatedAt(DateTime updatedAt)
    {
        ArgumentNullException.ThrowIfNull(updatedAt);
        Value = updatedAt;
    }

    public static implicit operator DateTime(UpdatedAt updatedAt)
    {
        return updatedAt.Value;
    }
}