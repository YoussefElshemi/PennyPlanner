namespace Core.ValueObjects;

public readonly record struct PasswordResetId
{
    public PasswordResetId(Guid passwordResetId)
    {
        ArgumentNullException.ThrowIfNull(passwordResetId);
        Value = passwordResetId;
    }

    private Guid Value { get; }

    public static implicit operator Guid(PasswordResetId passwordResetId)
    {
        return passwordResetId.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}