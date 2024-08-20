namespace Core.ValueObjects;

public readonly record struct CreatedBy
{
    private Guid Value { get; init; }

    public CreatedBy(Guid createdBy)
    {
        ArgumentNullException.ThrowIfNull(createdBy);
        Value = createdBy;
    }

    public static implicit operator Guid(CreatedBy createdBy)
    {
        return createdBy.Value;
    }
}