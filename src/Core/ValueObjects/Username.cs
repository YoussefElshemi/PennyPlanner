namespace Core.ValueObjects;

public readonly record struct Username
{
    public const string SystemUsername = "System";

    public Username(string username)
    {
        ArgumentNullException.ThrowIfNull(username);
        Value = username;
    }

    private string Value { get; }

    public static implicit operator string(Username username)
    {
        return username.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}