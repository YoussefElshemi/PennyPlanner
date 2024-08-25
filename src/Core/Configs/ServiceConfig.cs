namespace Core.Configs;

public record ServiceConfig
{
    public string PasswordResetUrl { get; init; } = null!;
}