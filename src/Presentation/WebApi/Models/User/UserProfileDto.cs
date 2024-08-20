namespace Presentation.WebApi.Models.User;

public class UserProfileDto
{
    public required string Username { get; set; }
    public required string EmailAddress { get; set; }
    public required string UserRole { get; set; }

}