using AutoFixture;
using Core.Models;
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
            PasswordHash = FakePasswordHash.CreateValid(fixture),
            CreatedBy = FakeCreatedBy.CreateValid(fixture),
            CreatedAt = FakeCreatedAt.CreateValid(fixture),
            UpdatedAt = FakeUpdatedAt.CreateValid(fixture)
        };
    }
}