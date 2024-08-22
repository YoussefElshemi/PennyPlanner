using Core.Models;
using Core.ValueObjects;
using Presentation.WebApi.Models.Authentication;

namespace Presentation.Mappers;

public static class RequestResetPasswordRequestMapper
{
    public static RequestResetPasswordRequest Map(RequestResetPasswordRequestDto requestResetPasswordRequestDto)
    {
        return new RequestResetPasswordRequest
        {
            EmailAddress = new EmailAddress(requestResetPasswordRequestDto.EmailAddress)
        };
    }
}