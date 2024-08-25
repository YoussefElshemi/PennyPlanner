using Core.Constants;

namespace Core.ValueObjects;

public readonly record struct RevokedAt
{
    public RevokedAt(DateTime revokedAt)
    {
        ArgumentNullException.ThrowIfNull(revokedAt);
        Value = revokedAt;
    }

    private DateTime Value { get; }

    public static implicit operator DateTime(RevokedAt revokedAt)
    {
        return revokedAt.Value;
    }

    public override string ToString()
    {
        return Value.ToString(DateTimeConstants.DateTimeFormat);
    }
}