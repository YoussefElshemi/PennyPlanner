namespace Presentation.WebApi.Models.Authentication;

public record RequestResetPasswordRequestDto
{
    public required string EmailAddress { get; init; }
}