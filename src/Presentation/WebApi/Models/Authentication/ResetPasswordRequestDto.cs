namespace Presentation.WebApi.Models.Authentication;

public record ResetPasswordRequestDto
{
    public required string PasswordResetToken { get; init; }
    public required string Password { get; init; }
    public required string ConfirmPassword { get; init; }
}