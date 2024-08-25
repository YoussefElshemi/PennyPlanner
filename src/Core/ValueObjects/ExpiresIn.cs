namespace Core.ValueObjects;

public readonly record struct ExpiresIn
{
    public ExpiresIn(int expiresIn)
    {
        ArgumentNullException.ThrowIfNull(expiresIn);
        Value = expiresIn;
    }

    private int Value { get; }

    public static implicit operator int(ExpiresIn expiresIn)
    {
        return expiresIn.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}