using Core.Models;
using Core.ValueObjects;
using Presentation.WebApi.Models.AuthenticatedUser;

namespace Presentation.Factories;

public static class UpdateUserRequestFactory
{
    public static User Map(User user, UpdateUserRequestDto updateUserRequestDto)
    {
        return user with
        {
            EmailAddress = updateUserRequestDto.EmailAddress is null ? user.EmailAddress : new EmailAddress(updateUserRequestDto.EmailAddress),
            Username = updateUserRequestDto.Username is null ? user.Username : new Username(updateUserRequestDto.Username)
        };
    }
}