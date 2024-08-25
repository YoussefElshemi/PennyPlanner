using AutoFixture;
using Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.Models;

public static class FakeLogin
{
    public static Login CreateValid(IFixture fixture)
    {
        return new Login
        {
            LoginId = FakeLoginId.CreateValid(fixture),
            UserId = FakeUserId.CreateValid(fixture),
            IpAddress = FakeIpAddress.CreateValid(fixture),
            RefreshToken = FakeRefreshToken.CreateValid(fixture),
            ExpiresAt = FakeExpiresAt.CreateValid(fixture),
            IsRevoked = FakeIsRevoked.CreateValid(fixture),
            User = FakeUser.CreateValid(fixture),
            CreatedBy = FakeUsername.CreateValid(),
            CreatedAt = FakeCreatedAt.CreateValid(fixture),
            UpdatedBy = FakeUsername.CreateValid(),
            UpdatedAt = FakeUpdatedAt.CreateValid(fixture)
        };
    }
}