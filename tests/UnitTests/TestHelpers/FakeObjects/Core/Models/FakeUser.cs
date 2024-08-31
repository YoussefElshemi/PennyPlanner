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
        var password = FakePassword.Valid;
        var passwordSalt = new PasswordSalt(Convert.ToBase64String(SecurityTokenHelper.GenerateSalt()));
        var passwordHash = AuthenticationService.HashPassword(new Password(password), passwordSalt);

        return new User
        {
            UserId = FakeUserId.CreateValid(fixture),
            Username = FakeUsername.CreateValid(),
            EmailAddress = FakeEmailAddress.CreateValid(),
            PasswordSalt = passwordSalt,
            PasswordHash = new PasswordHash(passwordHash),
            UserRole = fixture.Create<UserRole>(),
            IsDeleted = FakeIsDeleted.CreateValid(fixture),
            DeletedBy = FakeUsername.CreateValid(),
            DeletedAt = FakeDeletedAt.CreateValid(fixture),
            CreatedBy = FakeUsername.CreateValid(),
            CreatedAt = FakeCreatedAt.CreateValid(fixture),
            UpdatedBy = FakeUsername.CreateValid(),
            UpdatedAt = FakeUpdatedAt.CreateValid(fixture)
        };
    }
}