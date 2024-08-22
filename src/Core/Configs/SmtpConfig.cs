namespace Core.Configs;

public record SmtpConfig
{
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public string EmailAddress { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
}