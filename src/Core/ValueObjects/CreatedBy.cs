namespace Core.ValueObjects;

public record struct CreatedBy
{
    private string Value { get; init; }

    public CreatedBy(string createdBy)
    {
        ArgumentNullException.ThrowIfNull(createdBy);
        Value = createdBy;
    }

    public static implicit operator string(CreatedBy createdBy)
    {
        return createdBy.Value;
    }
}