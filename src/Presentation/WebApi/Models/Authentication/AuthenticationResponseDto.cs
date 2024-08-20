namespace Presentation.WebApi.Models.Authentication;

public record AuthenticationResponseDto
{
    public required string UserId { get; init; }

    public required string TokenType { get; init; }
    public required string AccessToken { get; init; }
    public required int ExpiresIn { get; init; }
}