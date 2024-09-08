using AutoFixture;
using Core.Configs;

namespace UnitTests.TestHelpers.FakeObjects.Core.Configs;

public static class FakeAppConfig
{
    public static AppConfig CreateValid(IFixture fixture)
    {
        return new AppConfig
        {
            JwtConfig = FakeJwtConfig.CreateValid(fixture),
            SmtpConfig = FakeSmtpConfig.CreateValid(fixture),
            ServiceConfig = FakeServiceConfig.CreateValid(fixture),
            PasswordResetConfig = FakePasswordResetConfig.CreateValid(fixture),
            AuthenticationConfig = FakeAuthenticationConfig.CreateValid(fixture)
        };
    }
}