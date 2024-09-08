using Core.Enums;
using Core.Models;
using Presentation.WebApi.UserManagement.Models.Requests;

namespace Presentation.Factories;

public static class UserManagementUpdateUserRequestFactory
{
    public static User Create(User user, UserManagementUpdateUserRequestDto userManagementUpdateUserRequestDto)
    {
        return UpdateUserRequestFactory.Create(user, userManagementUpdateUserRequestDto) with
        {
            UserRole = userManagementUpdateUserRequestDto.UserRole is null ? user.UserRole : Enum.Parse<UserRole>(userManagementUpdateUserRequestDto.UserRole)
        };
    }
}