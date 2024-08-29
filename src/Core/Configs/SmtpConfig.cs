namespace Core.Configs;

public record SmtpConfig
{
    public string Host { get; init; } = null!;
    public int Port { get; init; }
    public string EmailAddress { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Password { get; init; } = null!;
    public int NumberOfRetries { get; init; }
}