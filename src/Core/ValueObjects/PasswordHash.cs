namespace Core.ValueObjects;

public record struct PasswordHash
{
    private string Value { get; init; }

    public PasswordHash(string passwordHash)
    {
        ArgumentNullException.ThrowIfNull(passwordHash);
        Value = passwordHash;
    }

    public static implicit operator string(PasswordHash passwordHash)
    {
        return passwordHash.Value;
    }
}