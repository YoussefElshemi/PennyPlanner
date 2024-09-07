namespace Core.Configs;

public record AuthenticationConfig
{
    public int OneTimePasscodeLifetimeInMinutes { get; init; }
}