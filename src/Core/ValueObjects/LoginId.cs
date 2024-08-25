namespace Core.ValueObjects;

public readonly record struct LoginId
{
    public LoginId(Guid loginId)
    {
        ArgumentNullException.ThrowIfNull(loginId);
        Value = loginId;
    }

    private Guid Value { get; }

    public static implicit operator Guid(LoginId loginId)
    {
        return loginId.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}