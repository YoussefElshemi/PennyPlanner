using AutoFixture;
using Core.Configs;

namespace UnitTests.TestHelpers.FakeObjects.Core.Configs;

public static class FakeServiceConfig
{
    public static ServiceConfig CreateValid(IFixture fixture)
    {
        return new ServiceConfig
        {
            PasswordResetUrl = fixture.Create<string>()
        };
    }
}