namespace Core.ValueObjects;

public readonly record struct PasswordResetToken
{
    private string Value { get; init; }

    public PasswordResetToken(string passwordResetToken)
    {
        ArgumentNullException.ThrowIfNull(passwordResetToken);
        Value = passwordResetToken;
    }

    public static implicit operator string(PasswordResetToken passwordResetToken)
    {
        return passwordResetToken.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}