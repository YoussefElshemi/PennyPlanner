using Core.Models;
using Core.ValueObjects;
using Infrastructure.Entities;

namespace Infrastructure.Mappers;

public static class PasswordResetMapper
{
    public static PasswordReset MapFromEntity(PasswordResetEntity passwordResetEntity)
    {
        return new PasswordReset
        {
            PasswordResetId = new PasswordResetId(passwordResetEntity.PasswordResetId),
            UserId = new UserId(passwordResetEntity.UserId),
            User = UserMapper.MapFromEntity(passwordResetEntity.UserEntity),
            ResetToken = new PasswordResetToken(passwordResetEntity.ResetToken),
            IsUsed = new IsUsed(passwordResetEntity.IsUsed),
            CreatedAt = new CreatedAt(passwordResetEntity.CreatedAt),
            UpdatedAt = new UpdatedAt(passwordResetEntity.UpdatedAt),
        };
    }

    public static PasswordResetEntity MapToEntity(PasswordReset passwordReset)
    {
        return new PasswordResetEntity
        {
            PasswordResetId = passwordReset.PasswordResetId,
            UserId = passwordReset.UserId,
            ResetToken = passwordReset.ResetToken,
            IsUsed = passwordReset.IsUsed,
            CreatedAt = passwordReset.CreatedAt,
            UpdatedAt = passwordReset.UpdatedAt
        };
    }
}