namespace Core.ValueObjects;

public readonly record struct IpAddress
{
    public IpAddress(string ipAddress)
    {
        ArgumentNullException.ThrowIfNull(ipAddress);
        Value = ipAddress;
    }

    private string Value { get; }

    public static implicit operator string(IpAddress ipAddress)
    {
        return ipAddress.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}