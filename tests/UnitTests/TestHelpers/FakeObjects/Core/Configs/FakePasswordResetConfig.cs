using AutoFixture;
using Core.Configs;

namespace UnitTests.TestHelpers.FakeObjects.Core.Configs;

public static class FakePasswordResetConfig
{
    public static PasswordResetConfig CreateValid(IFixture fixture)
    {
        return new PasswordResetConfig
        {
            PasswordResetUrl = fixture.Create<string>(),
            PasswordResetTokenLifetimeInMinutes = fixture.Create<int>()
        };
    }
}