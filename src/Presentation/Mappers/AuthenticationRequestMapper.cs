using Core.Models;
using Core.ValueObjects;
using Presentation.WebApi.Models.Authentication;

namespace Presentation.Mappers;

public static class AuthenticationRequestMapper
{
    public static AuthenticationRequest Map(LoginRequestDto loginRequestDto)
    {
        return new AuthenticationRequest
        {
            Username = new Username(loginRequestDto.Username),
            Password = new Password(loginRequestDto.Password)
        };
    }
}