using Core.Enums;
using Core.Models;
using Core.ValueObjects;
using Infrastructure.Entities;

namespace Infrastructure.Mappers;

public static class UserMapper
{
    public static User MapFromEntity(UserEntity userEntity)
    {
        return new User
        {
            UserId = new UserId(userEntity.UserId),
            Username = new Username(userEntity.Username),
            EmailAddress = new EmailAddress(userEntity.EmailAddress),
            PasswordHash = new PasswordHash(userEntity.PasswordHash),
            PasswordSalt = new PasswordSalt(userEntity.PasswordSalt),
            UserRole = (UserRole)userEntity.UserRoleId,
            CreatedAt = new CreatedAt(userEntity.CreatedAt),
            UpdatedAt = new UpdatedAt(userEntity.UpdatedAt)
        };
    }

    public static UserEntity MapToEntity(User user)
    {
        return new UserEntity
        {
            UserId = user.UserId,
            Username = user.Username.ToString(),
            EmailAddress = user.EmailAddress.ToString(),
            PasswordHash = user.PasswordHash.ToString(),
            PasswordSalt = user.PasswordSalt.ToString(),
            UserRoleId = (int)user.UserRole,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}