namespace Core.ValueObjects;

public readonly record struct PasswordSalt
{
    private string Value { get; init; }

    public PasswordSalt(string passwordSalt)
    {
        ArgumentNullException.ThrowIfNull(passwordSalt);
        Value = passwordSalt;
    }

    public static implicit operator string(PasswordSalt passwordSalt)
    {
        return passwordSalt.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}