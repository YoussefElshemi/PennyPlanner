namespace Presentation.WebApi.Models.User;

public record UpdateUserRequestDto
{
    public string? Username { get; init; }
    public string? EmailAddress { get; init; }
}