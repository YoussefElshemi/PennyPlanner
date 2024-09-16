namespace Core.Configs;

public record BackgroundServiceConfig
{
    public bool EmailOutboxEnabled { get; init; }
    public int EmailOutboxIntervalInMs { get; init; }
}