using System.Collections.Immutable;
using Core.Enums;
using Infrastructure.Entities;

namespace Infrastructure.Configurations;

public static class SeedObjects
{
    private static readonly Guid UserId = Guid.NewGuid();
    public static readonly ImmutableArray<UserEntity> Users =
    [
        new UserEntity
        {
            UserId = UserId,
            Username = "admin",
            EmailAddress = "admin@admin.com",
            PasswordHash = string.Empty,
            PasswordSalt = string.Empty,
            UserRole = UserRole.Admin.ToString(),
            CreatedBy = UserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        }
    ];
}