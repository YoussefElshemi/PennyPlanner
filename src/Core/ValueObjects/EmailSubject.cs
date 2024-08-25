namespace Core.ValueObjects;

public readonly record struct EmailSubject
{
    public EmailSubject(string emailSubject)
    {
        ArgumentNullException.ThrowIfNull(emailSubject);
        Value = emailSubject;
    }

    private string Value { get; }

    public static implicit operator string(EmailSubject emailSubject)
    {
        return emailSubject.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}