using Core.Models;
using Presentation.WebApi.Models.User;

namespace Presentation.Mappers;

public static class UserProfileResponseMapper
{
    public static UserProfileResponseDto Map(User user)
    {
        return new UserProfileResponseDto
        {
            Username = user.Username.ToString(),
            EmailAddress = user.EmailAddress.ToString(),
            UserRole = user.UserRole.ToString()
        };
    }
}