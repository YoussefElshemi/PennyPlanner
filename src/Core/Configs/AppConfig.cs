namespace Core.Configs;

public record AppConfig
{
    public BackgroundServiceConfig BackgroundServiceConfig { get; init; } = null!;
    public ServiceConfig ServiceConfig { get; init; } = null!;
    public JwtConfig JwtConfig { get; init; } = null!;
    public AuthenticationConfig AuthenticationConfig { get; init; } = null!;
    public SmtpConfig SmtpConfig { get; init; } = null!;
    public PasswordResetConfig PasswordResetConfig { get; init; } = null!;
}