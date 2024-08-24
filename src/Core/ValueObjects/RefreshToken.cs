namespace Core.ValueObjects;

public readonly record struct RefreshToken
{
    private string Value { get; }

    public RefreshToken(string refreshToken)
    {
        ArgumentNullException.ThrowIfNull(refreshToken);
        Value = refreshToken;
    }

    public static implicit operator string(RefreshToken refreshToken)
    {
        return refreshToken.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}