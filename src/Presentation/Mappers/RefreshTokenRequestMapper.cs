using Core.Models;
using Core.ValueObjects;
using Presentation.WebApi.Models.Authentication;

namespace Presentation.Mappers;

public static class RefreshTokenRequestMapper
{
    public static RefreshTokenRequest Map(RefreshTokenRequestDto refreshTokenRequestDto, string ipAddress)
    {
        return new RefreshTokenRequest
        {
            RefreshToken = new RefreshToken(refreshTokenRequestDto.RefreshToken),
            IpAddress = new IpAddress(ipAddress)
        };
    }
}