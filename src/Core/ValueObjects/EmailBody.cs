namespace Core.ValueObjects;

public readonly record struct EmailBody
{
    private string Value { get; }

    public EmailBody(string emailBody)
    {
        ArgumentNullException.ThrowIfNull(emailBody);
        Value = emailBody;
    }

    public static implicit operator string(EmailBody emailBody)
    {
        return emailBody.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}