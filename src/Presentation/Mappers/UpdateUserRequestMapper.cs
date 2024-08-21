using Core.Models;
using Core.ValueObjects;
using Presentation.WebApi.Models.User;

namespace Presentation.Mappers;

public static class UpdateUserRequestMapper
{
    public static User Map(User user, UpdateUserRequestDto updateUserRequestDto)
    {
        return user with
        {
            EmailAddress = new EmailAddress(updateUserRequestDto.EmailAddress)
        };
    }
}