namespace Core.ValueObjects;

public readonly record struct PasswordResetId
{
    public PasswordResetId(Guid passwordResetId)
    {
        ArgumentNullException.ThrowIfNull(passwordResetId);
        Value = passwordResetId;
    }

    internal Guid Value { get; }

    public static implicit operator Guid(PasswordResetId passwordResetId)
    {
        return passwordResetId.Value;
    }

    public static implicit operator string(PasswordResetId passwordResetId)
    {
        return passwordResetId.Value.ToString();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}