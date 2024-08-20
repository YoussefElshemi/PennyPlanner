using Core.Models;
using Presentation.WebApi.Models.Authentication;

namespace Presentation.Mappers;

public static class AuthenticationResponseMapper
{
    public static AuthenticationResponseDto Map(AuthenticationResponse authenticationResponse)
    {
        return new AuthenticationResponseDto
        {
            UserId = authenticationResponse.UserId.ToString(),
            TokenType = authenticationResponse.TokenType.ToString(),
            AccessToken = authenticationResponse.AccessToken,
            ExpiresIn = authenticationResponse.ExpiresIn
        };
    }
}