using AutoFixture;
using Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.Models;

public static class FakeTwoFactorRequest
{
    public static TwoFactorRequest CreateValid(IFixture fixture)
    {
        return new TwoFactorRequest
        {
            Username = FakeUsername.CreateValid(),
            Passcode = FakePasscode.CreateValid(fixture),
            IpAddress = FakeIpAddress.CreateValid(fixture)
        };
    }
}