namespace Core.ValueObjects;

public readonly record struct ExpiresIn
{
    private int Value { get; init; }

    public ExpiresIn(int expiresIn)
    {
        ArgumentNullException.ThrowIfNull(expiresIn);
        Value = expiresIn;
    }

    public static implicit operator int(ExpiresIn expiresIn)
    {
        return expiresIn.Value;
    }
}