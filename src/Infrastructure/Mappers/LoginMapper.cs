using Core.Models;
using Core.ValueObjects;
using Infrastructure.Entities;

namespace Infrastructure.Mappers;

public static class LoginMapper
{
    public static Login MapFromEntity(LoginEntity loginEntity)
    {
        return new Login
        {
            LoginId = new LoginId(loginEntity.LoginId),
            UserId = new UserId(loginEntity.UserId),
            IpAddress = new IpAddress(loginEntity.IpAddress),
            RefreshToken = new RefreshToken(loginEntity.RefreshToken),
            ExpiresAt = new ExpiresAt(loginEntity.ExpiresAt),
            IsRevoked = new IsRevoked(loginEntity.IsRevoked),
            RevokedAt = loginEntity.RevokedAt is null ? null : new RevokedAt(loginEntity.RevokedAt.Value),
            CreatedAt = new CreatedAt(loginEntity.CreatedAt),
            UpdatedBy = new Username(loginEntity.UpdatedBy),
            UpdatedAt = new UpdatedAt(loginEntity.UpdatedAt),
            User = UserMapper.MapFromEntity(loginEntity.UserEntity)
        };
    }

    public static LoginEntity MapToEntity(Login login)
    {
        return new LoginEntity
        {
            LoginId = login.LoginId,
            UserId = login.UserId,
            IpAddress = login.IpAddress,
            RefreshToken = login.RefreshToken,
            ExpiresAt = login.ExpiresAt,
            IsRevoked = login.IsRevoked,
            RevokedAt = login.RevokedAt,
            CreatedAt = login.CreatedAt,
            UpdatedBy = login.UpdatedBy,
            UpdatedAt = login.UpdatedAt
        };
    }
}