using Core.Models;
using Core.ValueObjects;
using Presentation.WebApi.Models.Authentication;

namespace Presentation.Mappers;

public static class ResetPasswordRequestMapper
{
    public static ResetPasswordRequest Map(ResetPasswordRequestDto resetPasswordRequestDto)
    {
        return new ResetPasswordRequest
        {
            PasswordResetToken = new PasswordResetToken(resetPasswordRequestDto.PasswordResetToken),
            Password = new Password(resetPasswordRequestDto.Password)
        };
    }
}