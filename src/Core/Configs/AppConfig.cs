namespace Core.Configs;

public record AppConfig
{
    public JwtConfig JwtConfig { get; init; }
}