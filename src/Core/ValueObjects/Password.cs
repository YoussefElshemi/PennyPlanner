namespace Core.ValueObjects;

public readonly record struct Password
{
    public Password(string password)
    {
        ArgumentNullException.ThrowIfNull(password);
        Value = password;
    }

    private string Value { get; }

    public static implicit operator string(Password password)
    {
        return password.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}