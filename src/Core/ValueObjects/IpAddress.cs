namespace Core.ValueObjects;

public readonly record struct IpAddress
{
    private string Value { get; init; }

    public IpAddress(string ipAddress)
    {
        ArgumentNullException.ThrowIfNull(ipAddress);
        Value = ipAddress;
    }

    public static implicit operator string(IpAddress ipAddress)
    {
        return ipAddress.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}