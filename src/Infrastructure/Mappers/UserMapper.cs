using Core.Models;
using Core.ValueObjects;
using Infrastructure.Entities;

namespace Core.Mappers;

public static class UserMapper
{
    public static User MapFromEntity(UserEntity userEntity)
    {
        return new User
        {
            Id = new UserId(userEntity.Id),
            Username = new Username(userEntity.Username),
            EmailAddress = new EmailAddress(userEntity.EmailAddress),
            PasswordHash = new PasswordHash(userEntity.PasswordHash),
            CreatedBy = new CreatedBy(userEntity.CreatedBy),
            CreatedAt = new CreatedAt(userEntity.CreatedAt),
            UpdatedAt = new UpdatedAt(userEntity.UpdatedAt)
        };
    }

    public static UserEntity MapToEntity(User user)
    {
        return new UserEntity
        {
            Id = user.Id,
            Username = user.Username,
            EmailAddress = user.EmailAddress,
            PasswordHash = user.PasswordHash,
            CreatedBy = user.CreatedBy,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}