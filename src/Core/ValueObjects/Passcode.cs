namespace Core.ValueObjects;

public readonly record struct Passcode
{
    public Passcode(string passcode)
    {
        ArgumentNullException.ThrowIfNull(passcode);
        Value = passcode;
    }

    private string Value { get; }

    public static implicit operator string(Passcode passcode)
    {
        return passcode.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}