namespace Core.ValueObjects;

public readonly record struct OneTimePasscodeId
{
    public OneTimePasscodeId(Guid oneTimePasscodeId)
    {
        ArgumentNullException.ThrowIfNull(oneTimePasscodeId);
        Value = oneTimePasscodeId;
    }

    private Guid Value { get; }

    public static implicit operator Guid(OneTimePasscodeId oneTimePasscodeId)
    {
        return oneTimePasscodeId.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}