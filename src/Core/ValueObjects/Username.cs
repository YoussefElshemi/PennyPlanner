namespace Core.ValueObjects;

public readonly record struct Username
{
    private string Value { get; init; }

    public Username(string username)
    {
        ArgumentNullException.ThrowIfNull(username);
        Value = username;
    }

    public static implicit operator string(Username username)
    {
        return username.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}