namespace Core.ValueObjects;

public readonly record struct PasswordHash
{
    public PasswordHash(string passwordHash)
    {
        ArgumentNullException.ThrowIfNull(passwordHash);
        Value = passwordHash;
    }

    private string Value { get; }

    public static implicit operator string(PasswordHash passwordHash)
    {
        return passwordHash.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}