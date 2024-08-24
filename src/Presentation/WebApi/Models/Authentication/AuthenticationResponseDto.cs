namespace Presentation.WebApi.Models.Authentication;

public record AuthenticationResponseDto
{
    public required Guid UserId { get; init; }
    public required string TokenType { get; init; }
    public required string AccessToken { get; init; }
    public required int AccessTokenExpiresIn { get; init; }
    public required string RefreshToken { get; init; }
    public required int RefreshTokenExpiresIn { get; init; }
}