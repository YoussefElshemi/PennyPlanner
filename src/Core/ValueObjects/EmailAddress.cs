namespace Core.ValueObjects;

public readonly record struct EmailAddress
{
    public EmailAddress(string emailAddress)
    {
        ArgumentNullException.ThrowIfNull(emailAddress);
        Value = emailAddress;
    }

    private string Value { get; }

    public static implicit operator string(EmailAddress emailAddress)
    {
        return emailAddress.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}