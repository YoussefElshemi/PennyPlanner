namespace Core.ValueObjects;

public readonly record struct EmailId
{
    public EmailId(Guid emailrId)
    {
        ArgumentNullException.ThrowIfNull(emailrId);
        Value = emailrId;
    }

    private Guid Value { get; }

    public static implicit operator Guid(EmailId emailrId)
    {
        return emailrId.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}