namespace Presentation.WebApi.Models.User;

public record ChangePasswordRequestDto
{
    public required string Password { get; init; }
    public required string ConfirmPassword { get; init; }
}