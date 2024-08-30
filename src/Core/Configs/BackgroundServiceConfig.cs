namespace Core.Configs;

public record BackgroundServiceConfig
{
    public int EmailOutboxIntervalInMs { get; init; }
}