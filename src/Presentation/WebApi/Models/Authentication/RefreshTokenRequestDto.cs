namespace Presentation.WebApi.Models.Authentication;

public record RefreshTokenRequestDto
{
    public required string RefreshToken { get; init; }
}