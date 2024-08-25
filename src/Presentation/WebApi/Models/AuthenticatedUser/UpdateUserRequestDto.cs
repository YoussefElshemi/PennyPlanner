namespace Presentation.WebApi.Models.AuthenticatedUser;

public record UpdateUserRequestDto
{
    public string? Username { get; init; }
    public string? EmailAddress { get; init; }
}