namespace Core.ValueObjects;

public readonly record struct PasswordResetToken
{
    public PasswordResetToken(string passwordResetToken)
    {
        ArgumentNullException.ThrowIfNull(passwordResetToken);
        Value = passwordResetToken;
    }

    private string Value { get; }

    public static implicit operator string(PasswordResetToken passwordResetToken)
    {
        return passwordResetToken.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}