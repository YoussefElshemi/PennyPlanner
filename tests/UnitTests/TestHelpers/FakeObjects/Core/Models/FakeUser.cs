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
        return new User
        {
            UserId = FakeUserId.CreateValid(fixture),
            Username = FakeUsername.CreateValid(),
            EmailAddress = FakeEmailAddress.CreateValid(),
            PasswordSalt = FakePasswordSalt.CreateValid(fixture),
            PasswordHash = FakePasswordHash.CreateValid(fixture),
            UserRole = fixture.Create<UserRole>(),
            IsDeleted = FakeIsDeleted.CreateValid(fixture),
            DeletedBy = FakeUsername.CreateValid(),
            DeletedAt = FakeDeletedAt.CreateValid(fixture),
            CreatedAt = FakeCreatedAt.CreateValid(fixture),
            UpdatedBy = FakeUsername.CreateValid(),
            UpdatedAt = FakeUpdatedAt.CreateValid(fixture)
        };
    }
}