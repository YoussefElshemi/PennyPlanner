namespace Core.ValueObjects;

public readonly record struct LoginId
{
    internal Guid Value { get; }

    public LoginId(Guid loginId)
    {
        ArgumentNullException.ThrowIfNull(loginId);
        Value = loginId;
    }

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