namespace Presentation.WebApi.Models.Authentication;

public record RequestResetPasswordResponseDto
{
    public required string Message { get; init; }
}