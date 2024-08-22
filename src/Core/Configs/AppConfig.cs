namespace Core.Configs;

public record AppConfig
{
    public JwtConfig JwtConfig { get; init; } = null!;
    public SmtpConfig SmtpConfig { get; init; } = null!;
    public ServiceConfig ServiceConfig { get; init; } = null!;

}