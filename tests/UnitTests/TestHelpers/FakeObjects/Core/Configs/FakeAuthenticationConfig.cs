using AutoFixture;
using Core.Configs;

namespace UnitTests.TestHelpers.FakeObjects.Core.Configs;

public static class FakeAuthenticationConfig
{
    public static AuthenticationConfig CreateValid(IFixture fixture)
    {
        return new AuthenticationConfig
        {
            OneTimePasscodeLifetimeInMinutes = fixture.Create<int>()
        };
    }
}