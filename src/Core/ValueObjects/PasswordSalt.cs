namespace Core.ValueObjects;

public readonly record struct PasswordSalt
{
    public PasswordSalt(string passwordSalt)
    {
        ArgumentNullException.ThrowIfNull(passwordSalt);
        Value = passwordSalt;
    }

    private string Value { get; }

    public static implicit operator string(PasswordSalt passwordSalt)
    {
        return passwordSalt.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}