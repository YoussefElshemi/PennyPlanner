using Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.Models;

public static class FakeAuthenticationRequest
{
    public static AuthenticationRequest CreateValid()
    {
        return new AuthenticationRequest
        {
            Username = FakeUsername.CreateValid(),
            Password = FakePassword.CreateValid()
        };
    }
}