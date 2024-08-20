namespace Core.ValueObjects;

public readonly record struct AccessToken
{
    private string Value { get; init; }

    public AccessToken(string accessToken)
    {
        ArgumentNullException.ThrowIfNull(accessToken);
        Value = accessToken;
    }

    public static implicit operator string(AccessToken accessToken)
    {
        return accessToken.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}