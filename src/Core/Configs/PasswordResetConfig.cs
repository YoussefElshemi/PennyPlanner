namespace Core.Configs;

public record PasswordResetConfig
{
    public string PasswordResetUrl { get; init; } = null!;
    public int PasswordResetTokenLifetimeInMinutes { get; init; }
    public int NumberOfEmailRetries { get; init; }
}