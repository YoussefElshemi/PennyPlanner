using AutoFixture;
using Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.Models;

public static class FakeRefreshTokenRequest
{
    public static RefreshTokenRequest CreateValid(IFixture fixture)
    {
        return new RefreshTokenRequest
        {
            RefreshToken = FakeRefreshToken.CreateValid(fixture),
            IpAddress = FakeIpAddress.CreateValid(fixture)
        };
    }
}