using AutoFixture;
using Core.Enums;
using Core.Helpers;
using Core.Services;
using Core.ValueObjects;
using Infrastructure.Entities;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;

public static class FakeUserEntity
{
    public static UserEntity CreateValid(IFixture fixture)
    {
        var password = FakePassword.Valid;
        var passwordSalt = new PasswordSalt(Convert.ToBase64String(SecurityTokenHelper.GenerateSalt()));
        var passwordHash = AuthenticationService.HashPassword(new Password(password), passwordSalt);

        return new UserEntity
        {
            UserId = fixture.Create<Guid>(),
            Username = fixture.Create<string>(),
            EmailAddress = fixture.Create<string>(),
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            UserRoleId = (int)fixture.Create<UserRole>(),
            CreatedBy = fixture.Create<string>(),
            CreatedAt = fixture.Create<DateTime>(),
            UpdatedBy = fixture.Create<string>(),
            UpdatedAt = fixture.Create<DateTime>(),
            IsDeleted = false,
            DeletedBy = fixture.Create<string>(),
            DeletedAt = fixture.Create<DateTime>()
        };
    }
}