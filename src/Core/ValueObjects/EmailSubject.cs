namespace Core.ValueObjects;

public readonly record struct EmailSubject
{
    private string Value { get; }

    public EmailSubject(string emailSubject)
    {
        ArgumentNullException.ThrowIfNull(emailSubject);
        Value = emailSubject;
    }

    public static implicit operator string(EmailSubject emailSubject)
    {
        return emailSubject.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}