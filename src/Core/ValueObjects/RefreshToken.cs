namespace Core.ValueObjects;

public readonly record struct RefreshToken
{
    public RefreshToken(string refreshToken)
    {
        ArgumentNullException.ThrowIfNull(refreshToken);
        Value = refreshToken;
    }

    private string Value { get; }

    public static implicit operator string(RefreshToken refreshToken)
    {
        return refreshToken.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}