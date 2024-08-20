using Core.Models;
using Core.ValueObjects;
using Presentation.WebApi.Models.Authentication;

namespace Presentation.Mappers;

public static class CreateUserRequestMapper
{
    public static CreateUserRequest Map(RegisterRequestDto registerRequestDto)
    {
        return new CreateUserRequest
        {
            Username = new Username(registerRequestDto.Username),
            Password = new Password(registerRequestDto.Password),
            EmailAddress = new EmailAddress(registerRequestDto.EmailAddress)
        };
    }
}