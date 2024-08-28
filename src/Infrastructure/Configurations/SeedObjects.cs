using System.Collections.Immutable;
using Core.Enums;
using Core.Extensions;
using Core.ValueObjects;
using Infrastructure.Entities;

namespace Infrastructure.Configurations;

public static class SeedObjects
{
    private static readonly Guid UserId = Guid.NewGuid();
    public static readonly ImmutableArray<UserRoleEntity> UserRoles =
    [
        ..Enum.GetValues(typeof(UserRole))
            .Cast<UserRole>()
            .Select(x => new UserRoleEntity
            {
                UserRoleId = (int)x,
                Name = x.GetDescription(),
                CreatedBy = Username.SystemUsername,
                CreatedAt = TimeProvider.System.GetUtcNow().UtcDateTime,
                UpdatedBy = Username.SystemUsername,
                UpdatedAt = TimeProvider.System.GetUtcNow().UtcDateTime
            })
    ];

    public static readonly ImmutableArray<UserEntity> Users =
    [
        new UserEntity
        {
            UserId = UserId,
            Username = "admin",
            EmailAddress = "admin@admin.com",
            PasswordHash = string.Empty,
            PasswordSalt = string.Empty,
            UserRoleId = (int)UserRole.Admin,
            IsDeleted = false,
            DeletedBy = null,
            DeletedAt = null,
            CreatedBy = Username.SystemUsername,
            CreatedAt = TimeProvider.System.GetUtcNow().UtcDateTime,
            UpdatedBy = Username.SystemUsername,
            UpdatedAt = TimeProvider.System.GetUtcNow().UtcDateTime
        }
    ];
}