namespace Core.ValueObjects;

public record struct UpdatedAt
{
    private DateTime Value { get; init; }

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