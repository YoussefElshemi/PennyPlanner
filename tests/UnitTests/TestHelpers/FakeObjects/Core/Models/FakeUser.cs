using AutoFixture;
using Core.Enums;
using Core.Helpers;
using Core.Models;
using Core.Services;
using Core.ValueObjects;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.Models;

public static class FakeUser
{
    public static User CreateValid(IFixture fixture)
    {
        var passwordSalt = SecurityTokenHelper.GenerateSalt();
        var passwordHash = AuthenticationService.HashPassword(FakePassword.Valid, passwordSalt);

        return new User
        {
            UserId = FakeUserId.CreateValid(fixture),
            Username = FakeUsername.CreateValid(),
            EmailAddress = FakeEmailAddress.CreateValid(),
            PasswordSalt = new PasswordSalt(Convert.ToBase64String(passwordSalt)),
            PasswordHash = new PasswordHash(passwordHash),
            UserRole = fixture.Create<UserRole>(),
            CreatedAt = FakeCreatedAt.CreateValid(fixture),
            UpdatedAt = FakeUpdatedAt.CreateValid(fixture)
        };
    }
}