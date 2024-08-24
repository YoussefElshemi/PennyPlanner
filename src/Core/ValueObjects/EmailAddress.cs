namespace Core.ValueObjects;

public readonly record struct EmailAddress
{
    private string Value { get; }

    public EmailAddress(string emailAddress)
    {
        ArgumentNullException.ThrowIfNull(emailAddress);
        Value = emailAddress;
    }

    public static implicit operator string(EmailAddress emailAddress)
    {
        return emailAddress.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}