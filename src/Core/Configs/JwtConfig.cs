namespace Core.Configs;

public record JwtConfig
{
    public string Key { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int AccessTokenLifetimeInMinutes { get; init; }
    public int RefreshTokenLifetimeInMinutes { get; init; }
}