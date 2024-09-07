namespace Core.ValueObjects;

public readonly record struct IsTwoFactorAuthenticationEnabled
{
    public IsTwoFactorAuthenticationEnabled(bool isTwoFactorAuthenticationEnabled)
    {
        ArgumentNullException.ThrowIfNull(isTwoFactorAuthenticationEnabled);
        Value = isTwoFactorAuthenticationEnabled;
    }

    private bool Value { get; }

    public static implicit operator bool(IsTwoFactorAuthenticationEnabled isTwoFactorAuthenticationEnabled)
    {
        return isTwoFactorAuthenticationEnabled.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}