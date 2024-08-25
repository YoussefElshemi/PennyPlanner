namespace Core.ValueObjects;

public readonly record struct LoginId
{
    public LoginId(Guid loginId)
    {
        ArgumentNullException.ThrowIfNull(loginId);
        Value = loginId;
    }

    internal Guid Value { get; }

    public static implicit operator Guid(LoginId loginId)
    {
        return loginId.Value;
    }

    public static implicit operator string(LoginId loginId)
    {
        return loginId.Value.ToString();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}