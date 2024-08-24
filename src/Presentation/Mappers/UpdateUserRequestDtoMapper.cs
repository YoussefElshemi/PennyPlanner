using Presentation.WebApi.Models.UserManagement;

namespace Presentation.Mappers;

public static class UpdateUserRequestDtoMapper
{
    public static UpdateUserRequestDto Map(Guid userId, WebApi.Models.User.UpdateUserRequestDto updateUserRequestDto)
    {
        return new UpdateUserRequestDto
        {
            UserId = userId,
            Username = updateUserRequestDto.Username,
            EmailAddress = updateUserRequestDto.EmailAddress
        };
    }
}