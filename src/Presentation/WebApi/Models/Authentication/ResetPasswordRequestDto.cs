namespace Presentation.WebApi.Models.Authentication;

public record ResetPasswordRequestDto
{
    public required Guid PasswordResetToken { get; init; }
    public required string Password { get; init; }
    public required string ConfirmPassword { get; init; }
}