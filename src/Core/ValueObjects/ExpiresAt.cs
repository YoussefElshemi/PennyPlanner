namespace Core.ValueObjects;

public readonly record struct ExpiresAt
{
    public ExpiresAt(DateTime expiresAt)
    {
        ArgumentNullException.ThrowIfNull(expiresAt);
        Value = expiresAt;
    }

    private DateTime Value { get; }

    public static implicit operator DateTime(ExpiresAt expiresAt)
    {
        return expiresAt.Value;
    }
}