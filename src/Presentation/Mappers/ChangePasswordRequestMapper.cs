using Core.Models;
using Core.ValueObjects;
using Presentation.WebApi.Models.User;

namespace Presentation.Mappers;

public static class ChangePasswordRequestMapper
{
    public static ChangePasswordRequest Map(ChangePasswordRequestDto changePasswordRequestDto)
    {
        return new ChangePasswordRequest
        {
            Password = new Password(changePasswordRequestDto.Password)
        };
    }
}