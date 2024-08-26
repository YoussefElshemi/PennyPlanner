using Core.Models;
using Core.ValueObjects;
using Presentation.WebApi.AuthenticatedUser.Models.Requests;

namespace Presentation.Factories;

public static class UpdateUserRequestFactory
{
    public static User Create(User user, UpdateUserRequestDto updateUserRequestDto)
    {
        return user with
        {
            EmailAddress = updateUserRequestDto.EmailAddress is null ? user.EmailAddress : new EmailAddress(updateUserRequestDto.EmailAddress),
            Username = updateUserRequestDto.Username is null ? user.Username : new Username(updateUserRequestDto.Username)
        };
    }
}