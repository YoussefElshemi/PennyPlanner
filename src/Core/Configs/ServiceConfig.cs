namespace Core.Configs;

public record ServiceConfig
{
    public string ServiceName { get; init; } = null!;
    public Environment Environment { get; init;}
}