using Core.Models;
using Presentation.WebApi.Models.User;

namespace Presentation.Mappers;

public static class UserProfileResponseMapper
{
    public static UserProfileDto Map(User user)
    {
        return new UserProfileDto
        {
            Username = user.Username.ToString(),
            EmailAddress = user.EmailAddress.ToString(),
            UserRole = user.UserRole.ToString()
        };
    }
}