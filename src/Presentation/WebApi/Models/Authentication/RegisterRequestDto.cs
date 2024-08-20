namespace Presentation.WebApi.Models.Authentication;

public record RegisterRequestDto
{
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string ConfirmPassword { get; init; }
    public required string EmailAddress { get; init; }
}