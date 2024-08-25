namespace Core.ValueObjects;

public readonly record struct EmailBody
{
    public EmailBody(string emailBody)
    {
        ArgumentNullException.ThrowIfNull(emailBody);
        Value = emailBody;
    }

    private string Value { get; }

    public static implicit operator string(EmailBody emailBody)
    {
        return emailBody.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}