namespace Presentation.WebApi.Models.AuthenticatedUser;

public record UserProfileResponseDto
{
    public required Guid UserId { get; init; }
    public required string Username { get; init; }
    public required string EmailAddress { get; init; }
    public required string UserRole { get; init; }
    public required string CreatedBy { get; init; }
    public required string CreatedAt { get; init; }
    public required string UpdatedBy { get; init; }
    public required string UpdatedAt { get; init; }
}