namespace Core.ValueObjects;

public readonly record struct RevokedAt
{
    private DateTime? Value { get; }

    public RevokedAt(DateTime revokedAt)
    {
        ArgumentNullException.ThrowIfNull(revokedAt);
        Value = revokedAt;
    }

    public static implicit operator DateTime?(RevokedAt revokedAt)
    {
        return revokedAt.Value;
    }
}