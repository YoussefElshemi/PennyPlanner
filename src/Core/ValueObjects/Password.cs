namespace Core.ValueObjects;

public readonly record struct Password
{
    private string Value { get; init; }

    public Password(string password)
    {
        ArgumentNullException.ThrowIfNull(password);
        Value = password;
    }

    public static implicit operator string(Password password)
    {
        return password.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}