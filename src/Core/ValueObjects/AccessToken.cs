namespace Core.ValueObjects;

public readonly record struct AccessToken
{
    public AccessToken(string accessToken)
    {
        ArgumentNullException.ThrowIfNull(accessToken);
        Value = accessToken;
    }

    private string Value { get; }

    public static implicit operator string(AccessToken accessToken)
    {
        return accessToken.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}