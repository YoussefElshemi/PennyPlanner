namespace Core.ValueObjects;

public readonly record struct PasswordResetToken
{
    private Guid Value { get; init; }

    public PasswordResetToken(Guid passwordResetToken)
    {
        ArgumentNullException.ThrowIfNull(passwordResetToken);
        Value = passwordResetToken;
    }

    public static implicit operator Guid(PasswordResetToken passwordResetToken)
    {
        return passwordResetToken.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}