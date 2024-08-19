namespace Core.ValueObjects;

public record struct EmailAddress
{
    private string Value { get; init; }

    public EmailAddress(string emailAddress)
    {
        ArgumentNullException.ThrowIfNull(emailAddress);
        Value = emailAddress;
    }

    public static implicit operator string(EmailAddress emailAddress)
    {
        return emailAddress.Value;
    }
}