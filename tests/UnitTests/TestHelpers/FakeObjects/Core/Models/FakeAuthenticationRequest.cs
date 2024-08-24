using AutoFixture;
using Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.Models;

public static class FakeAuthenticationRequest
{
    public static AuthenticationRequest CreateValid(IFixture fixture)
    {
        return new AuthenticationRequest
        {
            Username = FakeUsername.CreateValid(),
            Password = FakePassword.CreateValid(),
            IpAddress = FakeIpAddress.CreateValid(fixture)
        };
    }
}