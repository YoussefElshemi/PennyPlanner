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
            IsDeleted = new IsDeleted(userEntity.IsDeleted),
            DeletedBy = userEntity.DeletedBy is null ? null : new Username(userEntity.DeletedBy),
            DeletedAt = userEntity.DeletedAt is null ? null : new DeletedAt(userEntity.DeletedAt.Value),
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
            IsDeleted = user.IsDeleted,
            DeletedBy = user.DeletedBy is null ? null : user.DeletedBy.ToString(),
            DeletedAt = user.DeletedAt,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}