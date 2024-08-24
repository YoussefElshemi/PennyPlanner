namespace Core.ValueObjects;

public readonly record struct ExpiresAt
{
    private DateTime Value { get; init; }

    public ExpiresAt(DateTime expiresAt)
    {
        ArgumentNullException.ThrowIfNull(expiresAt);
        Value = expiresAt;
    }

    public static implicit operator DateTime(ExpiresAt expiresAt)
    {
        return expiresAt.Value;
    }
}